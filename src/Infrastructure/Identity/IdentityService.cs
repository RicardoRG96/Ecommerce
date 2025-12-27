using Application.Abstractions.Common;
using Domain.Errors.Users;
using Infrastructure.Access;
using Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Identity
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public async Task<string?> GetUserNameAsync(long userId)
        {
            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            return user?.UserName;
        }

        public async Task<bool> IsInRoleAsync(long userId, string role)
        {
            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<Result<long>> CreateUserAsync(string userName, string email, string password)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded)
            {
                return Result.Failure<long>(UserErrors.CreationAttemptFailed);
            }

            IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Member);

            await transaction.CommitAsync();

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

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
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
