using SharedKernel;

namespace Domain.Errors.Products
{
    public static class ProductTaxCategoryErrors
    {
        public static Error NotFound(long productTaxCategoryId) => Error.NotFound(
            "ProductTaxCategory.NotFound",
            $"The ProductTaxCategory with the id = '{productTaxCategoryId}' was not found");

        public static Error NotFoundByName(string productTaxCategoryName) => Error.NotFound(
            "ProductTaxCategory.NotFoundByName",
            $"The ProductTaxCategory with the name = '{productTaxCategoryName}' was not found");
    }
}
