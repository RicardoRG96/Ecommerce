using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IProductAttributeValueRepository : IRepository<ProductAttributeValue>
    {
        Task<List<ProductAttributeValue>> GetSkuAttributesAsync(long skuId, CancellationToken cancellationToken);
        Task RemoveAttributeValueFromSku(
            long skuId, 
            long attributeValueId, 
            CancellationToken cancellationToken);
    }
}
