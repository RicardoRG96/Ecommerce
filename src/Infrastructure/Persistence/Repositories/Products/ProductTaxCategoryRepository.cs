using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductTaxCategoryRepository : Repository<ProductTaxCategory>, IProductTaxCategoryRepository
    {
        public ProductTaxCategoryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<ProductTaxCategory?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.ProductTaxCategories
                .Where(ptc => ptc.Name == name)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
