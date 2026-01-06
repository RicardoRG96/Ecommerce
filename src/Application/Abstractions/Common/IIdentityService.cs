using Application.Users.Users.Create;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Abstractions.Common
{
    public interface IIdentityService
    {
        Task<IDomainUser> GetUserByIdAsync(long id, CancellationToken cancellationToken);

        Task<IDomainUser> GetUserByEmailAsync(string email);

        Task<IDomainUser> GetByUsernameAsync(string username);

        Task<PaginatedList<IDomainUser>> GetAllUsersAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<Result<long>> CreateUserAsync(CreateUserCommand command);

        Task<Result> LoginUserAsync(string email, string password);

        void Update(IDomainUser user);

        Task<Result> DeleteUserAsync(long userId);

        Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken);

        Task<bool> IsUserNameUnique(string username, CancellationToken cancellation);
    }
}
