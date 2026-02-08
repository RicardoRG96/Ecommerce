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

        public static readonly Error InvalidOrderGallery = Error.Problem(
            "ProductSku.InvalidOrderGallery",
            "The SKU has an invalid order in the gallery");

        public static readonly Error DuplicateIdsInGallery = Error.Problem(
            "ProductSku.DuplicateIdsInGallery",
            "The SKU has duplicate IDs in the gallery");

        public static readonly Error ForeignItemInGallery = Error.Problem(
            "ProductSku.ForeignItemInGallery",
            "The SKU has a foreign item in the gallery");
    }
}
