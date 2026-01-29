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
            IEnumerable<long> attributeValueIds,
            CancellationToken cancellationToken)
        {
            if (attributeValueIds is null || !attributeValueIds.Any())
            {
                return new Dictionary<string, string>();
            }

            var attributeValuesList = attributeValueIds.ToList();

            var attributeValues = await _context.AttributeValues
                .Where(av => attributeValuesList.Contains(av.Id) && av.IsActive)
                .Include(av => av.Attribute)
                .Where(av => av.Attribute.IsActive)
                .OrderBy(av => av.Attribute.DisplayOrder)
                .ThenBy(av => av.DisplayOrder)
                .Select(av => new
                {
                    AttributeName = av.Attribute.Name ?? "Unknown",
                    AttributeValue = av.Value ?? string.Empty,
                    DisplayOrder = av.DisplayOrder
                })
                .ToListAsync(cancellationToken);

            // Group by attribute name and take first value if there are duplicates
            // This assumes each SKU should have only one value per attribute type
            return attributeValues
                .GroupBy(av => av.AttributeName)
                .ToDictionary(
                    g => g.Key,
                    g => g.First().AttributeValue
                );
        }
    }
}
