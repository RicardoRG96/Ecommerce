using SharedKernel;

namespace Domain.Errors.Users
{
    public static class UserErrors
    {
        public static Error NotFound(long userId) => Error.NotFound(
            "Users.NotFound",
            $"The user with the Id = '{userId}' was not found");

        public static Error Unauthorized() => Error.Failure(
            "Users.Unauthorized",
            "You are not authorized to perform this action");

        public static readonly Error NotFoundByEmail = Error.NotFound(
            "Users.NotFoundByEmail",
            "The user with the specified email was not found");

        public static readonly Error NotFoundByUsername = Error.NotFound(
            "Users.NotFoundByUsername",
            "The user with the specified username was not found");

        public static readonly Error EmailNotUnique = Error.Conflict(
            "Users.EmailNotUnique",
            "The provided email is not unique");

        public static readonly Error LoginAttemptFailed = Error.Failure(
            "Users.LoginFailed",
            "The credentials provided are not valid");

        public static readonly Error DeletionAttemptFailed = Error.Failure(
            "Users.DeletionFailed",
            "An error occurred while trying to delete the user");

        public static readonly Error CreationAttemptFailed = Error.Failure(
            "Users.CreationFailed",
            "An error occurred while trying to create the user");

        public static readonly Error UsernameNotUnique = Error.Conflict(
            "Users.UsernameNotUnique",
            "The provided userName is not unique");

        public static readonly Error HasNotLegalAge = Error.Problem(
            "Users.HasNotLegalAge",
            "You must be at least 18 years old to register.");
    }
}
