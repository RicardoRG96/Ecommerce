using Application.Abstractions.Messaging;

namespace Application.Users.Users.Login
{
    public sealed record LoginUserCommand(string Email, string Password) : ICommand;
}
