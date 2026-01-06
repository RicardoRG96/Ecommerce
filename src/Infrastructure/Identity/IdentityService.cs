using Application.Abstractions.Common;
using Application.Users.Users.Create;
using Domain.Entities.Users;
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
        private readonly ApplicationDbContext _dbContext;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IDomainUser> GetUserByIdAsync(long id, CancellationToken cancellationToken)
        {
            IDomainUser? user = await _userManager.Users
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            return user;
        }

        public async Task<IDomainUser> GetUserByEmailAsync(string email)
        {
            IDomainUser? user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<IDomainUser> GetByUsernameAsync(string username)
        {
            IDomainUser? user = await _userManager.FindByNameAsync(username);

            return user;
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
                return Result.Failure<long>(UserErrors.CreationAttemptFailed);
            }

            IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Member);

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
    }
}
