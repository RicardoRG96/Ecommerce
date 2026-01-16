namespace Web.Api.Endpoints.v1.Users.Role.UnassignRoles
{
    public sealed record UnassignRolesToUserRequest(IEnumerable<string> Roles);
}
