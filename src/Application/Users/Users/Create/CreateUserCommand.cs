using Application.Abstractions.Messaging;

namespace Application.Users.Users.Create
{
    public sealed record CreateUserCommand(
        string Email, 
        string Password) : ICommand<long>;
}
