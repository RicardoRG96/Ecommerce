using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductGalleryRepository : Repository<ProductGallery>, IProductGalleryRepository
    {
        public ProductGalleryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<List<ProductGallery>> GetByProductIdAsync(long productId, CancellationToken cancellationToken)
        {
            return await _context.ProductGalleries
                .Where(pg => pg.ProductId == productId)
                .Include(pg => pg.Product)
                .OrderBy(pg => pg.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductGallery?> GetPrimaryMediaAsync(long productId, CancellationToken cancellationToken)
        {
            return await _context.ProductGalleries
                .Where(pg => pg.ProductId == productId && pg.IsPrimary)
                .Include(pg => pg.Product)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
