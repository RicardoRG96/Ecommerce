namespace Web.Api.Endpoints.v1.Users.Role.AssignRoles
{
    public record AssignRolesToUserRequest(IEnumerable<string> Roles);
}
