using SharedKernel;

namespace Application.Abstractions.Common
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(long userId);

        Task<bool> IsInRoleAsync(long userId, string role);

        Task<Result<long>> CreateUserAsync(string userName, string email, string password);

        Task<Result> LoginUserAsync(string email, string password);

        Task<Result> DeleteUserAsync(long userId);
    }
}
