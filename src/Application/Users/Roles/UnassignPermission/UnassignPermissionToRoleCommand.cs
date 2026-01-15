using Application.Abstractions.Messaging;

namespace Application.Users.Roles.UnassignPermission
{
    public sealed record UnassignPermissionToRoleCommand(
        string Role,
        string Permission) : ICommand;
}
