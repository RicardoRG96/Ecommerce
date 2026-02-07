using SharedKernel;

namespace Application.Products.ProductGalleries.Common.Services
{
    public interface IProductGalleryValidator
    {
        Task<Result> ValidateProductIdExists(long productId, CancellationToken cancellationToken);
        Task<Result> ValidateSkuIdExists(long? skuId, CancellationToken cancellationToken);
        Task<Result> ValidateSkuBelongsToProduct(long productId, long? skuId, CancellationToken cancellationToken);
    }
}
