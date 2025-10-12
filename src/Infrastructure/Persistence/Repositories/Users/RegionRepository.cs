using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(ApplicationDbContext context)
            : base(context)
        {
            
        }

        public async Task<Region?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Regions
                .Where(r => r.Name == name)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
