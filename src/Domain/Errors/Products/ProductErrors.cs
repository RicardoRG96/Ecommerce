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
    }
}
