using SharedKernel;

namespace Domain.Errors.Products
{
    public static class BrandErrors
    {
        public static Error NotFound(long brandId) => Error.NotFound(
            "Brand.NotFound",
            $"The Brand with the id = '{brandId}' was not found");

        public static readonly Error DuplicatedBrandName = Error.Conflict(
            "Brand.DuplicatedBrandName",
            "The provided Name already exists");

        public static readonly Error NotUniqueSlug = Error.Conflict(
            "Brand.NotUniqueSlug",
            "The provided Slug is not unique");
    }
}
