using Application.Abstractions.Data.Repositories.Users;
using Domain.Users.Entities;
using Infrastructure.Persistence.Database;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class MunicipalityRepository : Repository<Municipality>, IMunicipalityRepository
    {
        public MunicipalityRepository(ApplicationDbContext context)
            : base(context)
        {
            
        }
    }
}
