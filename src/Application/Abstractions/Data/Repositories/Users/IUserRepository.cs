using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IUserRepository : IRepository<IDomainUser>
    {
        Task<IDomainUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        Task<IDomainUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken);

        Task<bool> IsUserNameUnique(string username, CancellationToken cancellationToken);
    }
}
