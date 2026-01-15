using Application.Abstractions.Messaging;

namespace Application.Users.Roles.Update
{
    public sealed record UpdateRoleCommand(
        long Id,
        string Name) : ICommand;
}
