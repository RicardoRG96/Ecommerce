namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IAttributeRepository : IRepository<Domain.Entities.Products.Attribute>
    {
        Task<List<Domain.Entities.Products.Attribute>> GetAllAsync();
    }
}
