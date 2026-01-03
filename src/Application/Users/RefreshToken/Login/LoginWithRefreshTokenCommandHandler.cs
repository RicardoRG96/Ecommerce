using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.RefreshToken.Login
{
    internal sealed class LoginWithRefreshTokenCommandHandler
        : ICommandHandler<LoginWithRefreshTokenCommand, Dictionary<string, string>>
    {
        public Task<Result<Dictionary<string, string>>> Handle(LoginWithRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
