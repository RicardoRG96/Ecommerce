using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.Common.Services
{
    public class AttributeValueValidator : IAttributeValueValidator
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;

        public AttributeValueValidator(
            IAttributeRepository attributeRepository, 
            IAttributeValueRepository attributeValueRepository)
        {
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<Result> ValidateAttributeIsActiveAsync(
            long attributeId, 
            CancellationToken cancellationToken = default)
        {
            Domain.Entities.Products.Attribute? attribute = await _attributeRepository.GetByIdAsync(attributeId, cancellationToken);

            if (attribute is null)
            {
                return Result.Failure(AttributeErrors.NotFound(attributeId));
            }

            if (!attribute.IsActive)
            {
                return Result.Failure(AttributeValueErrors.AttributeNotActive);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateAttributeValueIsUniqueAsync(
            long attributeId, 
            string value, 
            long? excludeAttributeValueId = null, 
            CancellationToken cancellationToken = default)
        {
            List<AttributeValue>? attributeValues = await _attributeValueRepository.GetValuesByAttributeAsync(
                attributeId, 
                cancellationToken);

            bool exists = attributeValues.Any(
                av => av.NormalizedValue == value.ToLowerInvariant() && 
                av.Id != excludeAttributeValueId);

            if (exists)
            {
                return Result.Failure(AttributeValueErrors.Duplicated);
            }

            return Result.Success();
        }

        public async Task<Result> ValidateSkuIsDisabledBeforeDeactivation(long attributeValueId, CancellationToken cancellationToken)
        {
            AttributeValue? attributeValue = await _attributeValueRepository.GetByIdWithRelatedEntitiesAsync(
                attributeValueId, 
                cancellationToken);

            if (attributeValue is null)
            {
                return Result.Failure(AttributeValueErrors.NotFound(attributeValueId));
            }

            bool hasActiveSkus = attributeValue.ProductAttributeValues
                .Any(pav => pav.ProductSku.IsActive);

            if (hasActiveSkus)
            {
                return Result.Failure(AttributeValueErrors.AttributeValueHasActiveSkus);
            }

            return Result.Success();
        }
    }
}
