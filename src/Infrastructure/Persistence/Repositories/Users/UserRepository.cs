using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) 
            : base(context) 
        { 
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(u => u.Username == username)
                .SingleOrDefaultAsync();
        }
    }
}
