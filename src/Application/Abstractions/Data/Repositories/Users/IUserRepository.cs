using Domain.Users.Entities;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmail(string email);
    }
}
