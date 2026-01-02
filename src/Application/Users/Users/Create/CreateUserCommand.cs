using Application.Abstractions.Messaging;

namespace Application.Users.Users.Create
{
    public sealed record CreateUserCommand(
        string Email,
        string Username,
        string Password) : ICommand<long>;
}
