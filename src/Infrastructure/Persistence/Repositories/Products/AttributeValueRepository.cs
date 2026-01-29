using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class AttributeValueRepository : Repository<AttributeValue>, IAttributeValueRepository
    {
        public AttributeValueRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<AttributeValue?> GetByIdWithRelatedEntitiesAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.AttributeValues
                .Where(av => av.Id == id)
                .Include(av => av.Attribute)
                .Include(av => av.ProductAttributeValues)
                .ThenInclude(pav => pav.ProductSku)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<AttributeValue>> GetValuesByAttributeAsync(long attributeId, CancellationToken cancellationToken)
        {
            return await _context.AttributeValues
                .Where(av => av.AttributeId == attributeId)
                .Include(av => av.Attribute)
                .Include(av => av.ProductAttributeValues)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyDictionary<string, string>> GetByIdsAsync(
            IEnumerable<long>? attributeValueIds,
            CancellationToken cancellationToken)
        {
            return _context.AttributeValues
                .Where()
        }
    }
}
