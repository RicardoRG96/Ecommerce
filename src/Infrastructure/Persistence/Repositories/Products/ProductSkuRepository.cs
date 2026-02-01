using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductSkuRepository : Repository<ProductSku>, IProductSkuRepository
    {
        public ProductSkuRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public Task<ProductSku?> GetByIdWithRelatedEntitiesAsync(long id, CancellationToken cancellationToken)
        {
            return _context.ProductSkus
                .Where(sku => sku.Id == id)
                .Include(sku => sku.Product)
                .ThenInclude(product => product.Brand)
                .Include(sku => sku.Product)
                .ThenInclude(product => product.Category)
                .Include(sku => sku.ProductAttributeValues)
                .ThenInclude(attrValue => attrValue.AttributeValue)
                .ThenInclude(attr => attr.Attribute)
                .Include(sku => sku.DiscountSkus)
                .ThenInclude(discountSku => discountSku.Discount)
                .Include(sku => sku.ProductGalleries)
                .Include(sku => sku.ProductSkuStocks)
                .ThenInclude(stock => stock.Warehouse)
                .FirstOrDefaultAsync(cancellationToken);    
        }

        public async Task<List<ProductSku>> GetSkusByProductIdAsync(long productId, CancellationToken cancellationToken)
        {
            return await _context.ProductSkus
                .Where(sku => sku.ProductId == productId)
                .Include(sku => sku.Product)
                .ThenInclude(product => product.Brand)
                .Include(sku => sku.Product)
                .ThenInclude(product => product.Category)
                .Include(sku => sku.ProductAttributeValues)
                .ThenInclude(attrValue => attrValue.AttributeValue)
                .Include(sku => sku.DiscountSkus)
                .ThenInclude(discountSku => discountSku.Discount)
                .Include(sku => sku.ProductGalleries)
                .Include(sku => sku.ProductSkuStocks)
                .ThenInclude(stock => stock.Warehouse)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductSku?> GetBySkuCodeAsync(string skuCode, CancellationToken cancellationToken)
        {
            return await _context.ProductSkus
                .Where(sku => sku.SkuCode == skuCode)
                .SingleOrDefaultAsync(cancellationToken);
        }
        
        public async Task<ProductSku?> GetByBarCodeAsync(string barCode, CancellationToken cancellationToken)
        {
            return await _context.ProductSkus
                .Where(sku => sku.BarCode == barCode)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
