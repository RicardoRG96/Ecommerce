using Domain.Entities.Users;

namespace Application.Abstractions.Data.Repositories.Users
{
    public interface IMunicipalityRepository : IRepository<Municipality>
    {
        Task<Municipality?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
