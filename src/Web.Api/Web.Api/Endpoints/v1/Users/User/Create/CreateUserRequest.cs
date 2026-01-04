namespace Web.Api.Endpoints.v1.Users.User.Create
{
    public record CreateUserRequest(
        string Avatar,
        string FirstName,
        string LastName,
        string Username,
        string Email,
        string Password,
        DateTime DateOfBirth,
        string PhoneNumber);
}
