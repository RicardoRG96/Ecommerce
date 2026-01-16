using Application.Abstractions.Messaging;

namespace Application.Users.RefreshTokens.Revoke
{
    public sealed record RevokeRefreshTokenCommand(long UserId) : ICommand;
}
