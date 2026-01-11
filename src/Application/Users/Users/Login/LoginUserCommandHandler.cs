using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Users.Login
{
    internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenProvider _tokenProvider;

        public LoginUserCommandHandler(
            IIdentityService identityService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            ITokenProvider tokenProvider)
        {
            _identityService = identityService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<LoginResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            Result identityResult = await _identityService.LoginUserAsync(command.Email, command.Password);

            if (identityResult.IsSuccess)
            {
                IDomainUser? user = await _identityService.GetUserByEmailAsync(command.Email);

                string accessToken = _tokenProvider.Create(user!);

                RefreshToken refreshToken = new()
                {
                    UserId = user!.Id,
                    Token = _tokenProvider.GenerateRefreshToken(),
                    ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
                };

                await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                LoginResponse response = new()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                };

                return Result.Success(response);
            }

            return Result.Failure<LoginResponse>(identityResult.Error);
        }
    }
}
