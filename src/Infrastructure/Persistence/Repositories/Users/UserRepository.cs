using Application.Abstractions.Data.Repositories;
using Application.Abstractions.Data.Repositories.Users;
using Domain.Users.Entities;
using Infrastructure.Persistence.Database;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) 
            : base(context) 
        { 
        }
        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<User>> IRepository<User>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<User?> IRepository<User>.GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
