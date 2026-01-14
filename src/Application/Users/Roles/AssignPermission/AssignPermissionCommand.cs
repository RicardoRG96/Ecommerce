using Application.Abstractions.Messaging;

namespace Application.Users.Roles.AssignPermission
{
    public sealed record AssignPermissionCommand(
        string Role,
        string Permission) : ICommand;
}
