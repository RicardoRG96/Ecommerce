using SharedKernel;

namespace Application.Abstractions.Common
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<Result<long>> CreateUserAsync(string userName, string email, string password);

        Task<Result> LoginUserAsync(string email, string password, bool rememberMe);

        Task<Result> DeleteUserAsync(string userId);
    }
}
