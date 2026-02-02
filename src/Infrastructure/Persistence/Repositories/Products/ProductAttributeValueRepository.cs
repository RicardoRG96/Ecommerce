using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductAttributeValueRepository : Repository<ProductAttributeValue>, IProductAttributeValueRepository
    {
        public ProductAttributeValueRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<List<ProductAttributeValue>> GetSkuAttributesAsync(
            long skuId, 
            CancellationToken cancellationToken)
        {
            return await _context.ProductAttributeValues
                .Where(pav => pav.ProductSkuId == skuId)
                .Include(pav => pav.ProductSku)
                .ThenInclude(ps => ps.Product)
                .ThenInclude(p => p.Category)
                .Include(pav => pav.ProductSku)
                .ThenInclude(ps => ps.Product)
                .ThenInclude(p => p.Brand)
                .Include(pav => pav.AttributeValue)
                .ThenInclude(av => av.Attribute)
                .ToListAsync(cancellationToken);
        }

        public async Task RemoveAttributeValueFromSku(
            long skuId, 
            long attributeValueId, 
            CancellationToken cancellationToken)
        {
            await _context.ProductAttributeValues
                .Where(pav => pav.ProductSkuId == skuId && pav.AttributeValueId == attributeValueId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
