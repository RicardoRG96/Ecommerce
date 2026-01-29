using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IAttributeValueRepository : IRepository<AttributeValue>
    {
        Task<List<AttributeValue>> GetValuesByAttributeAsync(long attributeId, CancellationToken cancellationToken);
        Task<AttributeValue?> GetByIdWithRelatedEntitiesAsync(long id, CancellationToken cancellationToken);
    }
}
