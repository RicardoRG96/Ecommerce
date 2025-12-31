using Infrastructure.Access;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Authorization
{
    internal sealed class PermissionProvider
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;

        public PermissionProvider(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<long>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<HashSet<string>> GetForUserIdAsync(long userId)
        {
            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            List<string> rolePermissionsClaims = await GetRolePermissionsClaims(user!);

            List<string> userPermissionsClaims = await GetUserPermissionsClaims(user!);

            HashSet<string> permissionsSet = [.. rolePermissionsClaims, .. userPermissionsClaims];

            return permissionsSet;
        }

        private async Task<List<string>> GetUserPermissionsClaims(ApplicationUser user)
        {
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user!);

            return userClaims
                .Where(c => c.Type == CustomClaimTypes.Permission)
                .Select(c => c.Value)
                .ToList();
        }

        private async Task<List<string>> GetRolePermissionsClaims(ApplicationUser user)
        {
            List<IdentityRole<long>> userRoles = await GetUserRoles(user);

            List<Claim> roleClaims = await GetRoleClaims(userRoles);

            return roleClaims
                .Where(c => c.Type == CustomClaimTypes.Permission)
                .Select(c => c.Value)
                .ToList();
        }

        private async Task<List<IdentityRole<long>>> GetUserRoles(ApplicationUser user)
        {
            IList<string> userRolesNames = await _userManager.GetRolesAsync(user!);

            List<IdentityRole<long>> userRoles = new();

            foreach (string userRoleName in userRolesNames)
            {
                IdentityRole<long>? role = await _roleManager.FindByNameAsync(userRoleName);
                userRoles.Add(role!);
            }

            return userRoles;
        }

        private async Task<List<Claim>> GetRoleClaims(List<IdentityRole<long>> identityRoles)
        {
            List<Claim> roleClaims = new List<Claim>();

            foreach (IdentityRole<long> identityRole in identityRoles)
            {
                List<Claim> claims = (List<Claim>)await _roleManager.GetClaimsAsync(identityRole);
                claims.ForEach(roleClaims.Add);
            }

            return roleClaims;
        }
    }
}
