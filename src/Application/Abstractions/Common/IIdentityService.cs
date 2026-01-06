using Application.Users.Users.Create;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Abstractions.Common
{
    public interface IIdentityService
    {
        Task<IDomainUser> GetUserByIdAsync(long id);

        Task<IDomainUser> GetUserByEmailAsync(string email);

        Task<IDomainUser> GetByUsernameAsync(string username);

        Task<Result<long>> CreateUserAsync(CreateUserCommand command);

        Task<Result> LoginUserAsync(string email, string password);

        Task<Result> DeleteUserAsync(long userId);
    }
}
