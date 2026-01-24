using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.Common.Services
{
    internal sealed class PublishProductValidator : IPublishProductValidator
    {
        private readonly IProductRepository _productRepository;

        public PublishProductValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> ValidateCanBePublishedAsync(long productId, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdIncludingRelatedEntitiesAsync(productId, cancellationToken);

            if (product is null)
            {
                return Result.Failure(ProductErrors.NotFound(productId));
            }

            if (product.Brand is null || !product.Brand.IsActive)
            {
                return Result.Failure(ProductErrors.BrandNotActive);
            }

            if (product.Category is null || !product.Category.IsActive)
            {
                return Result.Failure(ProductErrors.CategoryNotActive);
            }

            if (product.ProductTaxCategory is null || !product.ProductTaxCategory.IsActive)
            {
                return Result.Failure(ProductErrors.ProductTaxCategoryNotActive);
            }

            if (product.ProductSkus is null || product.ProductSkus.Count == 0)
            {
                return Result.Failure(ProductErrors.ProductMustHaveAtLeastOneSku);
            }

            if (product.ProductSkus.Any(sku => !sku.IsActive))
            {
                return Result.Failure(ProductErrors.ProductSkuNotActive);
            }

            if (product.ProductSkus.Any(sku => sku.Price <= 0))
            {
                return Result.Failure(ProductErrors.ProductSkuWithoutValidPrice);
            }

            if (product.ProductGalleries is null || product.ProductGalleries.Count == 0)
            {
                return Result.Failure(ProductErrors.ProductMustHaveAtLeastOneImage);
            }

            if (!product.ProductGalleries.Any(gallery => gallery.IsPrimary))
            {
                return Result.Failure(ProductErrors.ProductWithoutPrimaryImage);
            }

            return Result.Success();
        }
    }
}
