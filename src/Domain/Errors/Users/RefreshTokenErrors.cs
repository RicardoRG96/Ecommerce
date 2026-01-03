using SharedKernel;

namespace Domain.Errors.Users
{
    public static class RefreshTokenErrors
    {
        public static readonly Error ExpiredRefreshToken = Error.Failure(
            "RefreshToken.Expired",
            "The refresh token has expired");

        public static readonly Error NotTheLatestToken = Error.Conflict(
            "RefreshToken.Older",
            "This Refresh Token it's not the newest");
    }
}
