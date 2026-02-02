using SharedKernel;

namespace Application.Products.ProductAttributeValues.Common.Services
{
    public interface IProductAttributeValidator
    {
        Task<Result> ValidateAttributeValueIsActive(long attributeValueId, CancellationToken cancellationToken);
        Task<Result> ValidateSkuIsActive(long productSkuId, CancellationToken cancellationToken);
    }
}
