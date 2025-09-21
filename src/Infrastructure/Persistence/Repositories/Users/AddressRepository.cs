using Application.Abstractions.Data.Repositories.Users;
using Domain.Users.Entities;
using Infrastructure.Persistence.Database;

namespace Infrastructure.Persistence.Repositories.Users
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}
