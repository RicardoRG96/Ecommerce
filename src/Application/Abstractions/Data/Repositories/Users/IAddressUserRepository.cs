using Domain.Entities.Users;
using SharedKernel;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IAddressUserRepository : IRepository<AddressUser>
    {
        Task<PaginatedList<AddressUser>> GetByUserIdAsync(
            long userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<bool> SetAddressAsDefault(long userId, long addressId, CancellationToken cancellationToken);
    }
}
