namespace Infrastructure.Access
{
    public static class Permissions
    {
        public static class Users
        {
            public const string Read = "user:read";
            public const string Update = "user:update";
            public const string Delete = "user:delete";
            public const string Create = "user:create";
            public const string AssignRole = "user:assignRole";
            public const string AssignPermission = "user:assignPermission";
        }

        public static class Addresses
        {
            public const string Read = "address:read";
            public const string Update = "address:update";
            public const string Delete = "address:delete";
            public const string Create = "address:create";
        }

        public static class Countries
        {
            public const string Read = "country:read";
            public const string Update = "country:update";
            public const string Delete = "country:delete";
            public const string Create = "country:create";
        }

        public static class Regions
        {
            public const string Read = "region:read";
            public const string Update = "region:update";
            public const string Delete = "region:delete";
            public const string Create = "region:create";
        }

        public static class Municipalities
        {
            public const string Read = "municipality:read";
            public const string Update = "municipality:update";
            public const string Delete = "municipality:delete";
            public const string Create = "remunicipalitygion:create";
        }
    }
}
