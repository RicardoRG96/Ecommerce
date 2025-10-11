using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
