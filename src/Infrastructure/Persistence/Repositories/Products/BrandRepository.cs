using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Brands
                .Where(b => b.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Brand?>> GetFeaturedBrandsAsync(CancellationToken cancellationToken)
        {
            List<Brand> featuredBrands = await _context.Brands
                .Where(b => b.IsFeatured)
                .Where(b => b.IsActive)
                .ToListAsync(cancellationToken);

            return featuredBrands!;
        }
    }
}
