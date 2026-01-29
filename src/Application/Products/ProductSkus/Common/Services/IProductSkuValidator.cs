using SharedKernel;

namespace Application.Products.ProductSkus.Common.Services
{
    public interface IProductSkuValidator
    {
        Task<Result> ValidateProductIsPublishedAsync(
            long productId, 
            CancellationToken cancellationToken);

        Task<Result> ValidateSkuCodeIsUnique(
            string skuCode, 
            long? excludeProductSkuId = null, 
            CancellationToken cancellationToken = default);

        Task<Result> ValidateBarCodeIsUnique(
            string? barCode,
            long? excludeProductSkuId = null,
            CancellationToken cancellationToken = default);

        Task<Result> ValidateAttributeValueIsActive(
            long productSkuId,
            CancellationToken cancellationToken);

        Task<Result> ValidateSkuHasNoActiveOrders(
            long productSkuId,
            CancellationToken cancellationToken);
    }
}
