using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductSkus.Common.Services
{
    public class ProductSkuValidator : IProductSkuValidator
    {
        private readonly IProductSkuRepository _productSkuRepository;
        private readonly IProductRepository _productRepository;

        public ProductSkuValidator(
            IProductSkuRepository productSkuRepository, 
            IProductRepository productRepository)
        {
            _productSkuRepository = productSkuRepository;
            _productRepository = productRepository;
        }

        public async Task<Result> ValidateProductIsPublishedAsync(long productId, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdAsync(productId, cancellationToken);

            if (!product!.IsPublished)
            {
                return Result.Failure(ProductSkuErrors.ProductNotPublished);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateAttributeValuesAreActive(long productSkuId, CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdWithRelatedEntitiesAsync(productSkuId, cancellationToken);

            bool areAttributeValuesActive = productSku!.ProductAttributeValues
                .All(pav => pav.AttributeValue.IsActive);

            if (!areAttributeValuesActive)
            {
                return Result.Failure(ProductSkuErrors.AttributeValuesNotActive);
            }

            return Result.Success();
        }

        public Task<Result> ValidateBarCodeIsUnique(string? barCode, long? excludeProductSkuId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ValidateSkuCodeIsUnique(string skuCode, long? excludeProductSkuId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ValidateSkuHasNoActiveOrders(long productSkuId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
