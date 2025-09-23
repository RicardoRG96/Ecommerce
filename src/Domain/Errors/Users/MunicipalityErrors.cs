using SharedKernel;

namespace Domain.Errors.Users
{
    public static class MunicipalityErrors
    {
        public static Error NotFound(long municipalityId) => Error.NotFound(
            "Municipality.NotFound",
            $"The Region with the id = '{municipalityId}' was not found");

        public static Error Unauthorized() => Error.Failure(
            "Municipality.Unauthorized",
            "You are not authorized to perform this action");

        public static readonly Error NotFoundByName = Error.NotFound(
            "Municipality.NotFoundByName",
            "The Municipality with the specified name was not found");
    }
}
