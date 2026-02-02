using Domain.Entities.Products;
using SharedKernel;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetProductDetailByIdAsync(long id, CancellationToken cancellationToken);
        Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<Product?> GetByIdIncludingRelatedEntitiesAsync(long id, CancellationToken cancellationToken);
        Task<PaginatedList<Product>> GetAllPublishedProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<PaginatedList<Product>> GetAllProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
    }
}
