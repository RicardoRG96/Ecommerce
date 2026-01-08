using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address?> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Address?> GetByIdIncludingAddressUserAsync(long id, CancellationToken cancellationToken);
    }
}
