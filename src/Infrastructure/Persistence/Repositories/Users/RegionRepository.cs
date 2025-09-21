using Application.Abstractions.Data.Repositories.Users;
using Domain.Users.Entities;
using Infrastructure.Persistence.Database;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(ApplicationDbContext context)
            : base(context)
        {
            
        }
    }
}
