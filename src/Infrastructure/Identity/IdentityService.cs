using Application.Abstractions.Common;
using Domain.Errors.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager, 
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, 
            IAuthorizationService authorizationService, 
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
        }

        public async Task<string?> GetUserNameAsync(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            return user?.UserName;
        }

        public async Task<bool> IsInRoleAsync(long userId, string role)
        {
            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(long userId, string policyName)
        {
            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user is null)
            {
                return false;
            }

            ClaimsPrincipal? principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<Result<long>> CreateUserAsync(string userName, string email, string password)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            return Result.Success(user.Id);
        }

        public async Task<Result> LoginUserAsync(string email, string password, bool rememberMe)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(
                email, 
                password, 
                rememberMe, 
                false);

            return result.Succeeded ? Result.Success() : Result.Failure(UserErrors.LoginAttemptFailed);
        }

        public async Task<Result> DeleteUserAsync(long userId)
        {
            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(userId));
            }

            IdentityResult result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? Result.Success() : Result.Failure(UserErrors.DeletionAttemptFailed);
        }
    }
}
