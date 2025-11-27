namespace Web.Api.Endpoints.v1.Users.User.Update
{
    public sealed record UpdateUserRequest(
        string Avatar,
        string FirstName,
        string LastName,
        string PhoneNumber);
}
