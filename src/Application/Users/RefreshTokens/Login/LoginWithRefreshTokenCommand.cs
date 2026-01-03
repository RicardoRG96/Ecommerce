using Application.Abstractions.Messaging;

namespace Application.Users.RefreshTokens.Login
{
    public sealed record LoginWithRefreshTokenCommand(string RefreshToken, long UserId) 
        : ICommand<Dictionary<string, string>>;
}
