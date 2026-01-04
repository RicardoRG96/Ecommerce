using Application.Users.Users.Create;
using SharedKernel;

namespace Application.Abstractions.Common
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(long userId);

        Task<bool> IsInRoleAsync(long userId, string role);

        Task<Result<long>> CreateUserAsync(CreateUserCommand command);

        Task<Result> LoginUserAsync(string email, string password);

        Task<Result> DeleteUserAsync(long userId);
    }
}
