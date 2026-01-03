using Application.Abstractions.Messaging;

namespace Application.Users.RefreshToken.Login
{
    public sealed record LoginWithRefreshTokenCommand(string RefreshToken) : ICommand<Dictionary<string, string>>;
}
