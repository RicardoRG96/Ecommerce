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

        public Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.Brands.
                Where(b => b.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
