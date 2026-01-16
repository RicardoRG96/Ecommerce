using Infrastructure.Access;

namespace Api.FunctionalTests.Users.Role
{
    internal sealed class RoleHelper
    {
        public IEnumerable<string> GetRoleNames()
        {
            return new List<string>()
            {
                Roles.PlatformManager,
                Roles.CatalogManager
            };
        }
    }
}
