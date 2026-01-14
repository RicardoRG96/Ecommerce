using Application.Abstractions.Messaging;

namespace Application.Users.Roles.Unassign
{
    public sealed record UnassignRolesToUserCommand(
        IEnumerable<string> Roles,
        long UserId) : ICommand;
}
