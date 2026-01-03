using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.RefreshTokens.Revoke
{
    internal sealed class RevokeRefreshTokenCommandHandler : ICommandHandler<RevokeRefreshTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeRefreshTokenCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(command.UserId));
            }

            await _refreshTokenRepository.DeleteByUserId(command.UserId, cancellationToken);

            return Result.Success();
        }
    }
}
