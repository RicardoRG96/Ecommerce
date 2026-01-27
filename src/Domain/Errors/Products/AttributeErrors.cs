using SharedKernel;

namespace Domain.Errors.Products
{
    public static class AttributeErrors
    {
        public static Error NotFound(long attributeId) => Error.NotFound(
            "Attribute.NotFound",
            $"The Attribute with the id = '{attributeId}' was not found");

        public static Error NotFoundByName(string attributeName) => Error.NotFound(
            "Attribute.NotFoundByName",
            $"The Attribute with the name = '{attributeName}' was not found");

        public static readonly Error DuplicatedAttributeCode = Error.Conflict(
            "Attribute.DuplicatedAttributeCode",
            "The provided Code already exists");
    }
}
