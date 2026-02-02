using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductAttributeValues.Common.Services
{
    public class ProductAttributeValueValidator : IProductAttributeValueValidator
    {
        private readonly IProductSkuRepository _productSkuRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;

        public ProductAttributeValueValidator(
            IProductSkuRepository productSkuRepository, 
            IAttributeValueRepository attributeValueRepository)
        {
            _productSkuRepository = productSkuRepository;
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<Result> ValidateAttributeValueIsActive(long attributeValueId, CancellationToken cancellationToken)
        {
            AttributeValue? attributeValue = await _attributeValueRepository.GetByIdAsync(attributeValueId, cancellationToken);

            if (attributeValue is null)
            {
                return Result.Failure(AttributeValueErrors.NotFound(attributeValueId));
            }

            if (!attributeValue.IsActive)
            {
                return Result.Failure(ProductAttributeValueErrors.AttributeValueNotActive);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateSkuIsActive(long productSkuId, CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdAsync(productSkuId, cancellationToken);

            if (productSku is null)
            {
                return Result.Failure(ProductSkuErrors.NotFound(productSkuId));
            }

            if (!productSku.IsActive)
            {
                return Result.Failure(ProductAttributeValueErrors.ProductSkuNotActive);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateAttributeValueIsNotAlreadyAssignedToSku(
            long productSkuId,
            long attributeValueId,
            CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdWithRelatedEntitiesAsync(productSkuId, cancellationToken);

            if (productSku is null)
            {
                return Result.Failure(ProductSkuErrors.NotFound(productSkuId));
            }

            bool isAlreadyAssigned = productSku.ProductAttributeValues
                .Any(pav => pav.AttributeValueId == attributeValueId);

            if (isAlreadyAssigned)
            {
                return Result.Failure(ProductAttributeValueErrors.Duplicated);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateCanRemoveAttributeValueFromSku(
            long productSkuId,
            long attributeValueId,
            CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdWithRelatedEntitiesAsync(
                productSkuId, 
                cancellationToken);

            if (productSku is null)
            {
                return Result.Failure(ProductSkuErrors.NotFound(productSkuId));
            }

            AttributeValue? attributeValue = await _attributeValueRepository.GetByIdAsync(
                attributeValueId, 
                cancellationToken);

            if (attributeValue is null)
            {
                return Result.Failure(AttributeValueErrors.NotFound(attributeValueId));
            }

            bool isAssigned = productSku.ProductAttributeValues
                .Any(pav => pav.AttributeValueId == attributeValueId);

            if (!isAssigned)
            {
                return Result.Failure(ProductAttributeValueErrors.NotAssignedToSku);
            }

            return Result.Success();
        }
    }
}
