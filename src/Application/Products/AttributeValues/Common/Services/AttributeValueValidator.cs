using Application.Abstractions.Data.Repositories.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.Common.Services
{
    public class AttributeValueValidator : IAttributeValueValidator
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;

        public AttributeValueValidator(IAttributeRepository attributeRepository, IAttributeValueRepository attributeValueRepository)
        {
            _attributeRepository = attributeRepository;
            _attributeValueRepository = attributeValueRepository;
        }

        public Task<Result> ValidateAttributeIsActiveAsync(long attributeId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ValidateAttributeValueIsUniqueAsync(long attributeId, string value, long? excludeAttributeValueId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ValidateSkuIsDisabledBeforeDeactivation(long attributeValueId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
