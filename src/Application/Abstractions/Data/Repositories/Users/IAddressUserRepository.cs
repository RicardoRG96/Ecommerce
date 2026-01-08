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
    }
}
