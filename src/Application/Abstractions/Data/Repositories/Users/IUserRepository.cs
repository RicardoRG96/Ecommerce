using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
