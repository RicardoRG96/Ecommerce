using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

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
                .Include(au => au.Address)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginatedList<AddressUser>> GetByUserIdAsync(long userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<AddressUser> query = _context.AddressUsers.AsQueryable<AddressUser>()
                .Where(au => au.ApplicationUserId == userId)
                .Include(au => au.Address);

            int count = await query.CountAsync(cancellationToken);
            List<AddressUser> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return PaginatedList<AddressUser>.Create(items, count, pageNumber, pageSize);
        }
    }
}
