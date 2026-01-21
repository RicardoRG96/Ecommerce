using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<Brand?> GetByNameAsync(string name);
    }
}
