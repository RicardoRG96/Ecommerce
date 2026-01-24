using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IProductTaxCategoryRepository : IRepository<ProductTaxCategory>
    {
        Task<ProductTaxCategory?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
