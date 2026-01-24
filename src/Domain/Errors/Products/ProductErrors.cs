using SharedKernel;

namespace Domain.Errors.Products
{
    public static class ProductErrors
    {
        public static Error NotFound(long productId) => Error.NotFound(
            "Product.NotFound",
            $"The Product with the id = '{productId}' was not found");

        public static Error NotFoundByName(string productName) => Error.NotFound(
            "Product.NotFoundByName",
            $"The Product with the name = '{productName}' was not found");

        public static readonly Error DuplicatedProductName = Error.Problem(
            "Product.DuplicatedProductName",
            "The provided Name already exists");

        public static readonly Error BrandNotActive = Error.Problem(
            "Product.BrandNotActive",
            "The provided Brand for product creation is not active");

        public static readonly Error CategoryNotActive = Error.Problem(
            "Product.CategoryNotActive",
            "The provided Category for product creation is not active");

        public static readonly Error ProductTaxCategoryNotActive = Error.Problem(
            "Product.ProductTaxCategoryNotActive",
            "The provided Product Tax Category for product creation is not active");
    }
}
