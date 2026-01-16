using SharedKernel;

namespace Domain.Errors.Users
{
    public static class RoleErrors
    {
        public static Error NotFound(long roleId) => Error.NotFound(
            "Roles.NotFound",
            $"The role with the Id = '{roleId}' was not found");

        public static Error NotFoundByName(string roleName) => Error.NotFound(
            "Roles.NotFound",
            $"The role with the Name = '{roleName}' was not found");

        public static Error PermissionNotFound(string permission) => Error.NotFound(
            "Roles.PermissionNotFound",
            $"The permission with the Name = '{permission}' was not found");

        public static readonly Error PermissionAlreadyInRole = Error.Conflict(
            "Roles.PermissionAlreadyInRole",
            "The permission is already in the provided role");

        public static readonly Error PermissionIsNotInRole = Error.Conflict(
            "Roles.PermissionIsNotInRole",
            "The permission does not exist in the role.");

        public static readonly Error RoleAlreadyExists = Error.Conflict(
            "Roles.RoleAlreadyExists",
            "The provided role name already exists");
    }
}
