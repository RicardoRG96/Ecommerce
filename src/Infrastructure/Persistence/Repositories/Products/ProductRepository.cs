using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

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
    }
}
