using SharedKernel;

namespace Domain.Errors.Products
{
    public static class ProductErrors
    {
        public static Error NotFound(long productId) => Error.NotFound(
            "Product.NotFound",
            $"The Product with the id = '{productId}' was not found");

        public static readonly Error SearchNotFound = Error.NotFound(
            "Product.SearchNotFound",
            "No products were found matching the search criteria");

        public static Error NotFoundByName(string productName) => Error.NotFound(
            "Product.NotFoundByName",
            $"The Product with the name = '{productName}' was not found");

        public static readonly Error DuplicatedProductName = Error.Problem(
            "Product.DuplicatedProductName",
            "The provided Name already exists");

        public static readonly Error ProductNotActive = Error.Problem(
            "Product.ProductNotActive",
            "The Product is not active");

        public static readonly Error BrandNotActive = Error.Problem(
            "Product.BrandNotActive",
            "The provided Brand is not active");

        public static readonly Error CategoryNotActive = Error.Problem(
            "Product.CategoryNotActive",
            "The provided Category is not active");

        public static readonly Error ProductTaxCategoryNotActive = Error.Problem(
            "Product.ProductTaxCategoryNotActive",
            "The provided Product Tax Category is not active");

        public static readonly Error ProductSkuNotActive = Error.Problem(
            "Product.ProductSkuNotActive",
            "The provided Product SKU is not active");

        public static readonly Error ProductSkuWithoutValidPrice = Error.Problem(
            "Product.ProductSkuWithoutValidPrice",
            "The Product SKU does not have a valid price to be published");

        public static readonly Error ProductWithoutPrimaryImage = Error.Problem(
            "Product.ProductWithoutPrimaryImage",
            "The Product cannot be published without a primary image");

        public static readonly Error ProductMustHaveAtLeastOneSku = Error.Problem(
            "Product.ProductMustHaveAtLeastOneSku",
            "The Product must have at least one SKU to be published");

        public static readonly Error ProductMustHaveAtLeastOneImage = Error.Problem(
            "Product.ProductMustHaveAtLeastOneImage",
            "The Product must have at least one image to be published");

        public static readonly Error InvalidOrderGallery = Error.Problem(
            "Product.InvalidOrderGallery",
            "The Product has an invalid order in the gallery");

        public static readonly Error DuplicateIdsInGallery = Error.Problem(
            "Product.DuplicateIdsInGallery",
            "The Product has duplicate IDs in the gallery");

        public static readonly Error ForeignItemInGallery = Error.Problem(
            "Product.ForeignItemInGallery",
            "The Product has a foreign item in the gallery");
    }
}
