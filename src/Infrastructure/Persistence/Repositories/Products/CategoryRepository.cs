using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Where(c => c.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
