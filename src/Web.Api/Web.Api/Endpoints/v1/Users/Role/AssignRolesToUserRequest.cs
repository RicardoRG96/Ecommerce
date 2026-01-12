namespace Web.Api.Endpoints.v1.Users.Role
{
    public record AssignRolesToUserRequest(IEnumerable<string> Roles);
}
