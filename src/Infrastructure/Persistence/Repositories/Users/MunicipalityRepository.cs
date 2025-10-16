using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class MunicipalityRepository : Repository<Municipality>, IMunicipalityRepository
    {
        public MunicipalityRepository(ApplicationDbContext context)
            : base(context)
        {
            
        }

        public async Task<Municipality?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Municipalities
                .Where(m => m.Name == name)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
