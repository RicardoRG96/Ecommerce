using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class UserRepository : Repository<IDomainUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) 
            : base(context) 
        { 
        }

        public async Task<IDomainUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IDomainUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => u.UserName == username)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
