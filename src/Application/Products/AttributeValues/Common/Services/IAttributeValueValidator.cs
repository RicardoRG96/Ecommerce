using SharedKernel;

namespace Application.Products.AttributeValues.Common.Services
{
    public interface IAttributeValueValidator
    {
        Task<Result> ValidateAttributeIsActiveAsync(
            long attributeId,
            CancellationToken cancellationToken = default);

        Task<Result> ValidateAttributeValueIsUniqueAsync(
            long attributeId,
            string value,
            long? excludeAttributeValueId = null,
            CancellationToken cancellationToken = default);

        Task<Result> ValidateSkuIsDisabledBeforeDeactivation(
            long attributeValueId,
            CancellationToken cancellationToken);
    }
}
