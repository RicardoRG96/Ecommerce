namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IAttributeRepository : IRepository<Domain.Entities.Products.Attribute>
    {
        Task<List<Domain.Entities.Products.Attribute>> GetAllAsync(CancellationToken cancellationToken);
        Task<Domain.Entities.Products.Attribute?> GetByCodeAsync(string code, CancellationToken cancellationToken);
    }
}
