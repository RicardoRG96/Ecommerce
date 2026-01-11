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
        : ICommandHandler<LoginWithRefreshTokenCommand, RefreshTokenResponse>
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

        public async Task<Result<RefreshTokenResponse>> Handle(LoginWithRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.GetByTokenAsync(
                command.RefreshToken, command.UserId, cancellationToken);

            if (refreshToken is null)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.NotFound);
            }

            if (refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.ExpiredRefreshToken);
            }

            bool isLatestToken = await _refreshTokenRepository.IsLatestTokenAsync(
                refreshToken.Token, command.UserId, cancellationToken);

            if (!isLatestToken)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.NotTheLatestToken);
            }

            IDomainUser? user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            string accessToken = _tokenProvider.Create(user!);

            refreshToken.Token = _tokenProvider.GenerateRefreshToken();
            refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

            _refreshTokenRepository.Update(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            RefreshTokenResponse response = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };

            return Result.Success(response);
        }
    }
}
