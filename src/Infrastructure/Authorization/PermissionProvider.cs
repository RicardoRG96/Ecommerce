namespace Infrastructure.Authorization
{
    internal sealed class PermissionProvider
    {
        public Task<HashSet<string>> GetForUserIdAsync(long userId)
        {
            // TODO: Here you'll implement your logic to fetch permissions.
            // por ejemplo puede ser fetchear roles o claims con identity usando UserManager
            HashSet<string> permissionsSet = [];

            return Task.FromResult(permissionsSet);
        }
    }
}
