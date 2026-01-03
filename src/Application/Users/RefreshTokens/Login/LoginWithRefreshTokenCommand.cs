using Application.Abstractions.Messaging;

namespace Application.Users.RefreshTokens.Login
{
    public sealed record LoginWithRefreshTokenCommand(string RefreshToken) : ICommand<Dictionary<string, string>>;
}
