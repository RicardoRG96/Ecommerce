using SharedKernel;

namespace Domain.Errors.Products
{
    public static class AttributeValueErrors
    {
        public static Error NotFound(long attributeValueId) => Error.NotFound(
            "AttributeValue.NotFound",
            $"The AttributeValue with the id = '{attributeValueId}' was not found");

        public static readonly Error AttributeNotActive = Error.Problem(
            "AttributeValue.AttributeNotActive",
            "The provided Attribute is not active");

        public static readonly Error Duplicated = Error.Problem(
            "AttributeValue.Duplicated",
            "The provided AttributeValue already exists");

        public static readonly Error AttributeValueHasActiveSkus = Error.Problem(
            "AttributeValue.AttributeValueHasActiveSkus",
            "The provided AttributeValue has active SKUs and cannot be deactivated");
    }
}
