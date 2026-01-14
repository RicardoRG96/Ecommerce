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
    }
}
