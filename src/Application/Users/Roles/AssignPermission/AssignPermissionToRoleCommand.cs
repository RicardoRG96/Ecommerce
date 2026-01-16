using Application.Abstractions.Messaging;

namespace Application.Users.Roles.AssignPermission
{
    public sealed record AssignPermissionToRoleCommand(
        string Role,
        string Permission) : ICommand;
}
