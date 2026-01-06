using Application.Abstractions.Authentication;
using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.RefreshTokens.Login
{
    internal sealed class LoginWithRefreshTokenCommandHandler
        : ICommandHandler<LoginWithRefreshTokenCommand, Dictionary<string, string>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IIdentityService _identityService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUnitOfWork _unitOfWork;

        public LoginWithRefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IIdentityService identityService,
            ITokenProvider tokenProvider,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _identityService = identityService;
            _tokenProvider = tokenProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Dictionary<string, string>>> Handle(LoginWithRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.GetByTokenAsync(
                command.RefreshToken, command.UserId, cancellationToken);

            if (refreshToken is null)
            {
                return Result.Failure<Dictionary<string, string>>(RefreshTokenErrors.NotFound);
            }

            if (refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                return Result.Failure<Dictionary<string, string>>(RefreshTokenErrors.ExpiredRefreshToken);
            }

            bool isLatestToken = await _refreshTokenRepository.IsLatestTokenAsync(
                refreshToken.Token, cancellationToken);

            if (!isLatestToken)
            {
                return Result.Failure<Dictionary<string, string>>(RefreshTokenErrors.NotTheLatestToken);
            }

            IDomainUser? user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            string accessToken = _tokenProvider.Create(user!);

            refreshToken.Token = _tokenProvider.GenerateRefreshToken();
            refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

            _refreshTokenRepository.Update(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            Dictionary<string, string> response = new()
            {
                { "accessToken", accessToken },
                { "refreshToken", refreshToken.Token }
            };

            return Result.Success(response);
        }
    }
}
