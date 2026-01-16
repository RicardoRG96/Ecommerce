using System.Reflection;

namespace Infrastructure.Access
{
    public sealed class PermissionHelper
    {
        public static IEnumerable<string> GetAllPermissions()
        {
            return typeof(Permissions)
                .GetNestedTypes()
                .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Select(f => f.GetValue(null)?.ToString())
                .Where(v => !string.IsNullOrWhiteSpace(v))!;
        }
    }
}
