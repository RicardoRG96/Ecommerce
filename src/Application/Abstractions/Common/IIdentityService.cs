using Application.Users.Users.Create;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Abstractions.Common
{
    public interface IIdentityService
    {
        Task<IDomainUser> GetUserByIdAsync(long id);

        Task<IDomainUser> GetUserByEmailAsync(string email);

        Task<string?> GetUserNameAsync(long userId);

        Task<bool> IsInRoleAsync(long userId, string role);

        Task<Result<long>> CreateUserAsync(CreateUserCommand command);

        Task<Result> LoginUserAsync(string email, string password);

        Task<Result> DeleteUserAsync(long userId);
    }
}
