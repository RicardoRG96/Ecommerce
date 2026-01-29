using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IProductSkuRepository : IRepository<ProductSku>
    {
        Task<ProductSku?> GetByIdWithRelatedEntitiesAsync(long id, CancellationToken cancellationToken);
        Task<List<ProductSku>?> GetSkusByProductIdAsync(long productId, CancellationToken cancellationToken);
        Task<ProductSku?> GetBySkuCodeAsync(string skuCode, CancellationToken cancellationToken);
        Task<ProductSku?> GetByBarCodeAsync(string barCode, CancellationToken cancellationToken);
    }
}
