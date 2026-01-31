using SharedKernel;

namespace Domain.Errors.Products
{
    public static class ProductSkuErrors
    {
        public static Error NotFound(long skuId) => Error.NotFound(
            "ProductSku.NotFound",
            $"The SKU with the id = '{skuId}' was not found");

        public static Error NotFoundByName(string skuName) => Error.NotFound(
            "ProductSku.NotFoundByName",
            $"The SKU with the name = '{skuName}' was not found");

        public static readonly Error DuplicatedSkuCode = Error.Problem(
            "ProductSku.DuplicatedSkuCode",
            "The provided SKU code already exists");

        public static readonly Error DuplicatedBarCode = Error.Problem(
            "ProductSku.DuplicatedBarCode",
            "The provided Bar Code already exists");

        public static readonly Error SkuNotActive = Error.Problem(
            "ProductSku.SkuNotActive",
            "The SKU is not active");

        public static readonly Error ProductNotPublished = Error.Problem(
            "ProductSku.ProductNotPublished",
            "The Product is not published");

        public static readonly Error AttributeValuesNotActive = Error.Problem(
            "ProductSku.AttributeValuesNotActive",
            "The SKU has attribute values that are not active");
    }
}
