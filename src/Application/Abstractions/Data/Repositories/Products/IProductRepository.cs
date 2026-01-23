using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetProductDetailByIdAsync(long id, CancellationToken cancellationToken);
        Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
