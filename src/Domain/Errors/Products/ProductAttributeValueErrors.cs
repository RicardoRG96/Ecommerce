using SharedKernel;

namespace Domain.Errors.Products
{
    public class ProductAttributeValueErrors
    {
        public static Error NotFound(long ProductAttributeValueId) => Error.NotFound(
            "ProductAttributeValue.NotFound",
            $"The ProductAttributeValue with the id = '{ProductAttributeValueId}' was not found");

        public static readonly Error AttributeValueNotActive = Error.Problem(
            "ProductAttributeValue.AttributeValueNotActive",
            "The provided AttributeValue is not active");

        public static readonly Error ProductSkuNotActive = Error.Problem(
            "ProductAttributeValue.ProductSkuNotActive",
            "The provided ProductSku is not active");

        public static readonly Error Duplicated = Error.Problem(
            "ProductAttributeValue.Duplicated",
            "The provided AttributeValue has already assigned to the SKU");

        public static readonly Error NotAssignedToSku = Error.Problem(
            "ProductAttributeValue.NotAssignedToSku",
            "The provided AttributeValue is not assigned to the SKU");
    }
}
