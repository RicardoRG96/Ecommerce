using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task<Address?> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            return await _context.Addresses
                .Where(a => a.Title == title)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
