using Application.Abstractions.Messaging;

namespace Application.Users.Users.Delete
{
    public sealed record DeleteUserCommand(long UserId) : ICommand;
}
