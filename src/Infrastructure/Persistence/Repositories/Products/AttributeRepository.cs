using Application.Abstractions.Data.Repositories.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class AttributeRepository : Repository<Domain.Entities.Products.Attribute>, IAttributeRepository
    {
        public AttributeRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<List<Domain.Entities.Products.Attribute>> GetAllAsync()
        {
            return await _context.Attributes
                .Include(a => a.AttributeValues)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Products.Attribute?> GetByCodeAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.Attributes
                .Where(a => a .Code == code)
                .Include(a => a.AttributeValues)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
