using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
    }
}
