using SharedKernel;

namespace Application.Products.ProductGalleries.Common.Services
{
    public interface IProductGalleryValidator
    {
        Task<Result> ValidateProductIdExistsAsync(long productId, CancellationToken cancellationToken);
        Task<Result> ValidateSkuIdExistsAsync(long? skuId, CancellationToken cancellationToken);
        Task<Result> ValidateSkuBelongsToProductAsync(long productId, long? skuId, CancellationToken cancellationToken);
    }
}
