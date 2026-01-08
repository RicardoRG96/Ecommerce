using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IAddressUserRepository : IRepository<AddressUser>
    {
        Task<List<AddressUser>> GetByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
