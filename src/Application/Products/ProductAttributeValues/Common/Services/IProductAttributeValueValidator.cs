using SharedKernel;

namespace Application.Products.ProductAttributeValues.Common.Services
{
    public interface IProductAttributeValueValidator
    {
        Task<Result> ValidateAttributeValueIsActive(long attributeValueId, CancellationToken cancellationToken);
        Task<Result> ValidateSkuIsActive(long productSkuId, CancellationToken cancellationToken);
        Task<Result> ValidateAttributeValueIsNotAlreadyAssignedToSku(
            long productSkuId, 
            long attributeValueId, 
            CancellationToken cancellationToken);
    }
}
