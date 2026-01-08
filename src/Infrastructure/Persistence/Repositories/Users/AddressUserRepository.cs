using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class AddressUserRepository : Repository<AddressUser>, IAddressUserRepository
    {
        public AddressUserRepository(ApplicationDbContext context)
            : base(context)
        {
            
        }

        public async Task<List<AddressUser>> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            return await _context.AddressUsers
                .Where(au => au.ApplicationUserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
