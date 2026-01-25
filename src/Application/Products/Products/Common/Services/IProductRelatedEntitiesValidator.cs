using SharedKernel;

namespace Application.Products.Products.Common.Services
{
    public interface IProductRelatedEntitiesValidator
    {
        Task<Result> ValidateRelatedEntitiesAsync(
            long brandId,
            long categoryId,
            long productTaxCategoryId,
            CancellationToken cancellationToken);

        Task<Result> ValidateProductNameUniquenessAsync(
            string name,
            long? excludeProductId = null,
            CancellationToken cancellationToken = default);
    }
}
