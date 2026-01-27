using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public Task<Product?> GetProductDetailByIdAsync(long id, CancellationToken cancellationToken)
        {
            return _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductSkus)
                .ThenInclude(p => p.ProductAttributeValues)
                .ThenInclude(pa => pa.AttributeValue)
                .ThenInclude(av => av.Attribute)
                .Include(p => p.ProductGalleries)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdIncludingRelatedEntitiesAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductTaxCategory)
                .Include(p => p.ProductSkus)
                .Include(p => p.DiscountProducts)
                .Include(p => p.ProductGalleries)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PaginatedList<Product>> GetAllPublishedProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _context.Products.AsQueryable()
                .Where(p => p.IsPublished)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductSkus)
                .ThenInclude(p => p.ProductAttributeValues)
                .ThenInclude(pa => pa.AttributeValue)
                .ThenInclude(av => av.Attribute)
                .Include(p => p.ProductGalleries);

            int count = await query.CountAsync(cancellationToken);
            List<Product> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return PaginatedList<Product>.Create(items, count, pageNumber, pageSize);
        }

        public async Task<PaginatedList<Product>> GetAllProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _context.Products.AsQueryable<Product>()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductSkus)
                .ThenInclude(p => p.ProductAttributeValues)
                .ThenInclude(pa => pa.AttributeValue)
                .ThenInclude(av => av.Attribute)
                .Include(p => p.ProductGalleries);

            int count = await query.CountAsync(cancellationToken);
            List<Product> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return PaginatedList<Product>.Create(items, count, pageNumber, pageSize);
        }
    }
}
