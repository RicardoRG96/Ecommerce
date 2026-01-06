using Application.Abstractions.Common;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.RefreshTokens.Revoke
{
    internal sealed class RevokeRefreshTokenCommandHandler : ICommandHandler<RevokeRefreshTokenCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeRefreshTokenCommandHandler(
            IIdentityService identityService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _identityService = identityService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _identityService.GetUserByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            await _refreshTokenRepository.DeleteByUserIdAsync(command.UserId, cancellationToken);

            return Result.Success();
        }
    }
}
