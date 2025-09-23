using SharedKernel;

namespace Domain.Errors.Users
{
    public static class CountryErrors
    {
        public static Error NotFound(long countryId) => Error.NotFound(
            "Country.NotFound",
            $"The Country with the id = '{countryId}' was not found");

        public static Error Unauthorized() => Error.Failure(
            "Country.Unauthorized",
            "You are not authorized to perform this action");

        public static readonly Error NotFoundByName = Error.NotFound(
            "Country.NotFoundByName",
            "The country with the specified name was not found");
    }
}
