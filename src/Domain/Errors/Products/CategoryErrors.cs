using SharedKernel;

namespace Domain.Errors.Products
{
    public static class CategoryErrors
    {
        public static Error NotFound(long categoryId) => Error.NotFound(
            "Category.NotFound",
            $"The Category with the id = '{categoryId}' was not found");

        public static Error NotFoundByName(string categoryName) => Error.NotFound(
            "Category.NotFoundByName",
            $"The Category with the name = '{categoryName}' was not found");

        public static readonly Error CategoryAlreadyExists = Error.Conflict(
            "Category.DuplicatedCategoryName",
            "There's already a Category with the same name.");
    }
}
