using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Users.Login
{
    internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, Dictionary<string, string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly ITokenProvider _tokenProvider;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IIdentityService identityService,
            ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<Dictionary<string, string>>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            Result identityResult = await _identityService.LoginUserAsync(command.Email, command.Password);

            if (identityResult.IsSuccess)
            {
                IDomainUser? user = await _userRepository.GetUserByEmailAsync(command.Email, cancellationToken);

                string accessToken = _tokenProvider.Create(user!);

                RefreshToken refreshToken = new()
                {
                    UserId = user!.Id,
                    Token = _tokenProvider.GenerateRefreshToken(),
                    ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
                };

                await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                Dictionary<string, string> response = new()
                {
                    { "accessToken", accessToken },
                    { "refreshToken", refreshToken.Token }
                };

                return Result.Success(response);
            }

            return Result.Failure<Dictionary<string, string>>(identityResult.Error);
        }
    }
}
