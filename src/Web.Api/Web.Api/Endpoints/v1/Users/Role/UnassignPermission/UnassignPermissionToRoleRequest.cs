namespace Web.Api.Endpoints.v1.Users.Role.UnassignPermission
{
    public record UnassignPermissionToRoleRequest(string RoleName, string PermissionName);
}
