using Application.Abstractions.Messaging;

namespace Application.Users.Roles.Delete
{
    public sealed record DeleteRoleCommand(long Id) : ICommand;
}
