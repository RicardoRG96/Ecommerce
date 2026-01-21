using Application.Abstractions.Data.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext context)
            : base(context)
        { 
        }

        public async Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Countries
                .Where(c => c.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
