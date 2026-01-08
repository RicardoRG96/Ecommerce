namespace Infrastructure.Access
{
    public sealed class RolePermissions
    {
        public static readonly Dictionary<string, IReadOnlyCollection<string>> Map =
            new()
            {
                ["Admin"] = PermissionHelper.GetAllPermissions().ToList(),

                ["PlatformManager"] =
                [
                    Permissions.Users.Read,
                    Permissions.Users.Create,
                    Permissions.Users.Update,
                    Permissions.Users.Delete,
                    Permissions.Users.AssignRole,
                    Permissions.Users.AssignPermission,

                    Permissions.Countries.Read,
                    Permissions.Countries.Create,
                    Permissions.Countries.Update,
                    Permissions.Countries.Delete,

                    Permissions.Regions.Read,
                    Permissions.Regions.Create,
                    Permissions.Regions.Update,
                    Permissions.Regions.Delete,

                    Permissions.Municipalities.Read,
                    Permissions.Municipalities.Create,
                    Permissions.Municipalities.Update,
                    Permissions.Municipalities.Delete,
                ],

                ["CustomerSupport"] =
                [
                    Permissions.Users.Read
                ]
            };
    }
}
