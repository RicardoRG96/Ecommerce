using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductGalleries.Common.Services
{
    public sealed class ProductGalleryValidator : IProductGalleryValidator
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSkuRepository _productSkuRepository;

        public ProductGalleryValidator(
            IProductRepository productRepository, 
            IProductSkuRepository productSkuRepository)
        {
            _productRepository = productRepository;
            _productSkuRepository = productSkuRepository;
        }

        public async Task<Result> ValidateProductIdExistsAsync(long productId, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdAsync(productId, cancellationToken);

            if (product is null)
            {
                return Result.Failure(ProductGalleryErrors.ProductNotFound);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateSkuIdExistsAsync(long? skuId, CancellationToken cancellationToken)
        {
            if (skuId is null)
            {
                return Result.Success();
            }

            ProductSku? sku = await _productSkuRepository.GetByIdAsync(skuId.Value, cancellationToken);

            if (sku is null)
            {
                return Result.Failure(ProductGalleryErrors.SkuNotFound);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateSkuBelongsToProductAsync(long productId, long? skuId, CancellationToken cancellationToken)
        {
            if (skuId is null)
            {
                return Result.Success();
            }

            Product? product = await _productRepository.GetByIdAsync(productId, cancellationToken);

            if (product is null)
            {
                return Result.Failure(ProductGalleryErrors.ProductNotFound);
            }

            if (!product.ProductSkus.Any(s => s.Id == skuId.Value))
            {
                return Result.Failure(ProductGalleryErrors.SkuNotBelongsToProduct);
            }

            return Result.Success();
        }
    }
}
