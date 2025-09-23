using SharedKernel;

namespace Domain.Errors.Users
{
    public static class RegionErrors
    {
        public static Error NotFound(long regionId) => Error.NotFound(
            "Region.NotFound",
            $"The Region with the id = '{regionId}' was not found");

        public static Error Unauthorized() => Error.Failure(
            "Region.Unauthorized",
            "You are not authorized to perform this action");

        public static readonly Error NotFoundByName = Error.NotFound(
            "Region.NotFoundByName",
            "The Region with the specified name was not found");
    }
}
