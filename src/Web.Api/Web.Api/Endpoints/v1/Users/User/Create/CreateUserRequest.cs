namespace Web.Api.Endpoints.v1.Users.User.Create
{
    public record CreateUserRequest(
        string Username,
        string Email,
        string Password);
}
