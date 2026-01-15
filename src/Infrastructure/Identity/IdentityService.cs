using Application.Abstractions.Common;
using Application.Users.Roles.Assign;
using Application.Users.Roles.AssignPermission;
using Application.Users.Roles.Unassign;
using Application.Users.Roles.UnassignPermission;
using Application.Users.Users.Create;
using Application.Users.Users.UpdatePassword;
using Domain.Entities.Users;
using Domain.Errors.Users;
using Infrastructure.Access;
using Infrastructure.Persistence.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<long>> roleManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<IDomainUser> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _userManager.Users
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            return user!;
        }

        public async Task<IDomainUser> GetUserByEmailAsync(string email)
        {
            IDomainUser? user = await _userManager.FindByEmailAsync(email);

            return user!;
        }

        public async Task<IDomainUser> GetByUsernameAsync(string username)
        {
            IDomainUser? user = await _userManager.FindByNameAsync(username);

            return user!;
        }

        public async Task<PaginatedList<IDomainUser>> GetAllUsersAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<IDomainUser> query = _dbContext.Users.AsQueryable<IDomainUser>();

            int count = await query.CountAsync(cancellationToken);

            List<IDomainUser> items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return PaginatedList<IDomainUser>.Create(items, count, pageNumber, pageSize);
        }

        public async Task<Result<long>> CreateUserAsync(CreateUserCommand command)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            ApplicationUser user = new ApplicationUser
            {
                Avatar = command.Avatar,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName,
                Email = command.Email,
                DateOfBirth = command.DateOfBirth,
                PhoneNumber = command.PhoneNumber
            };

            if (!user.HasLegalAge())
            {
                return Result.Failure<long>(UserErrors.HasNotLegalAge);
            }

            IdentityResult identityResult = await _userManager.CreateAsync(user, command.Password);
            
            if (!identityResult.Succeeded)
            {
                Error[] identityErrors = [.. identityResult.Errors
                    .Select(e => e.Description)
                    .Select(d => new Error("Users.Creation", d, ErrorType.Validation))];

                ValidationError validationErrors = new(identityErrors);

                return Result.Failure<long>(validationErrors);
            }

            IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Customer);

            await transaction.CommitAsync();

            return Result.Success(user.Id);
        }

        public async Task<Result> LoginUserAsync(string email, string password)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFoundByEmail);
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return Result.Failure(UserErrors.LoginAttemptFailed);
            }

            return Result.Success();
        }

        public void Update(IDomainUser user)
        {
            ApplicationUser applicationUser = (ApplicationUser)user;
            _dbContext.Users.Update(applicationUser);
        }

        public async Task<Result> DeleteUserAsync(IDomainUser user)
        {
            ApplicationUser applicationUser = (ApplicationUser)user;

            IdentityResult result = await _userManager.DeleteAsync(applicationUser);

            return result.Succeeded ? Result.Success() : Result.Failure(UserErrors.DeletionAttemptFailed);
        }

        public async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _dbContext.Users
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync(cancellationToken);

            return user is null;
        }

        public async Task<bool> IsUserNameUnique(string username, CancellationToken cancellation)
        {
            IDomainUser? user = await _dbContext.Users
                .Where(u => u.UserName == username)
                .SingleOrDefaultAsync(cancellation);

            return user is null;
        }

        public async Task<Result> UpdatePasswordAsync(UpdatePasswordCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            IdentityResult identityResult = await _userManager.ChangePasswordAsync(user!, command.CurrentPassword, command.NewPassword);

            if (!identityResult.Succeeded)
            {
                Error[] identityErrors = [.. identityResult.Errors
                    .Select(e => e.Description)
                    .Select(d => new Error("Users.UpdatePassword", d, ErrorType.Validation))];

                ValidationError validationErrors = new(identityErrors);

                return Result.Failure<long>(validationErrors);
            }

            return Result.Success();
        }

        public async Task<Result> AddRolesToUserAsync(AssignRolesToUserCommand command, CancellationToken cancellationToken)
        {
            foreach(string role in command.Roles)
            {
                IdentityRole<long>? existingRole = await _roleManager.FindByNameAsync(role);

                if (existingRole is null)
                {
                    return Result.Failure(RoleErrors.NotFoundByName(role));
                }
            }

            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            IEnumerable<string> roles = command.Roles;

            IdentityResult identityResult = await _userManager.AddToRolesAsync(user!, roles);

            if (!identityResult.Succeeded)
            {
                Error[] identityErrors = [.. identityResult.Errors
                    .Select(e => e.Description)
                    .Select(d => new Error("Roles.AddRoleToUser", d, ErrorType.Validation))];

                ValidationError validationErrors = new(identityErrors);

                return Result.Failure(validationErrors);
            }

            return Result.Success();
        }

        public async Task<Result> RemoveRolesFromUserAsync(
            UnassignRolesToUserCommand command, CancellationToken cancellationToken)
        {
            foreach (string role in command.Roles)
            {
                IdentityRole<long>? existingRole = await _roleManager.FindByNameAsync(role);

                if (existingRole is null)
                {
                    return Result.Failure(RoleErrors.NotFoundByName(role));
                }
            }

            ApplicationUser? user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .SingleOrDefaultAsync(cancellationToken);

            IEnumerable<string> roles = command.Roles;

            IdentityResult identityResult = await _userManager.RemoveFromRolesAsync(user!, roles);

            if (!identityResult.Succeeded)
            {
                Error[] identityErrors = [.. identityResult.Errors
                    .Select(e => e.Description)
                    .Select(d => new Error("Roles.RemoveFromUser", d, ErrorType.Validation))];

                ValidationError validationErrors = new(identityErrors);

                return Result.Failure(validationErrors);
            }

            return Result.Success();
        }

        public async Task<Result> AddPermissionToRole(AssignPermissionToRoleCommand command, CancellationToken cancellationToken)
        {
            IdentityRole<long>? role = await _roleManager.FindByNameAsync(command.Role);

            if (role is null)
            {
                return Result.Failure(RoleErrors.NotFoundByName(command.Role));
            }

            IEnumerable<string> permissionsClaims = PermissionHelper.GetAllPermissions();

            if (!permissionsClaims.Contains(command.Permission))
            {
                return Result.Failure(RoleErrors.PermissionNotFound(command.Permission));
            }

            IdentityResult identityResult = await _roleManager.AddClaimAsync(
                role, 
                new Claim(CustomClaimTypes.Permission, command.Permission));

            if (!identityResult.Succeeded)
            {
                Error[] identityErrors = [.. identityResult.Errors
                    .Select(e => e.Description)
                    .Select(d => new Error("Roles.AddPermission", d, ErrorType.Validation))];

                ValidationError validationErrors = new(identityErrors);

                return Result.Failure(validationErrors);
            }

            return Result.Success();
        }

        public async Task<Result> RemovePermissionToRole(UnassignPermissionToRoleCommand command, CancellationToken cancellationToken)
        {
            IdentityRole<long>? role = await _roleManager.FindByNameAsync(command.Role);

            if (role is null)
            {
                return Result.Failure(RoleErrors.NotFoundByName(command.Role));
            }

            IEnumerable<string> permissionsClaims = PermissionHelper.GetAllPermissions();

            if (!permissionsClaims.Contains(command.Permission))
            {
                return Result.Failure(RoleErrors.PermissionNotFound(command.Permission));
            }

            IdentityResult identityResult = await _roleManager.RemoveClaimAsync(
                role, 
                new Claim(CustomClaimTypes.Permission, command.Permission));

            if (!identityResult.Succeeded)
            {
                Error[] identityErrors = [.. identityResult.Errors
                    .Select(e => e.Description)
                    .Select(d => new Error("Roles.AddPermission", d, ErrorType.Validation))];

                ValidationError validationErrors = new(identityErrors);

                return Result.Failure(validationErrors);
            }

            return Result.Success();
        }
    }
}
