namespace Api.FunctionalTests.Common
{
    public static class ApiRoutes
    {
        private const string ApiBase = "api/v1";

        public static class Users
        {
            public const string Base = $"{ApiBase}/users";
        }

        public static class Products
        {
            public const string Base = $"{ApiBase}/products";
        }

        public static class Addresses
        {
            public const string Base = $"{ApiBase}/addresses";
        }

        public static class Locations
        {
            public const string Countries = $"{ApiBase}/countries";
            public const string Regions = $"{ApiBase}/regions";
            public const string Municipalities = $"{ApiBase}/municipalities";
        }

        public static class Admin
        {
            public const string Roles = $"{ApiBase}/admin/roles";
        }

        public static class Brands
        {
            public const string Base = $"{ApiBase}/brands";
        }

        public static class Categories
        {
            public const string Base = $"{ApiBase}/categories";
        }
    }
}
