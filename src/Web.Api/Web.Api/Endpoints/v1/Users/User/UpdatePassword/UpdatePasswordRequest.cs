namespace Web.Api.Endpoints.v1.Users.User.UpdatePassword
{
    public sealed record UpdatePasswordRequest(
        string CurrentPassword,
        string NewPassword);
}
