using SharedKernel;

namespace Domain.Errors.Users
{
    public static class AddressErrors
    {
        public static Error NotFound(long addressId) => Error.NotFound(
            "Address.NotFound",
            $"The Address with the id = '{addressId}' was not found");

        public static Error Unauthorized() => Error.Failure(
            "Address.Unauthorized",
            "You are not authorized to perform this action");
    }
}
