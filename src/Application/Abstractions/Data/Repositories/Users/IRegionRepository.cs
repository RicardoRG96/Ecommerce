using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IRegionRepository : IRepository<Region>
    {
        Task<Region?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
