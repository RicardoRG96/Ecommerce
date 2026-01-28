namespace Web.Api.Endpoints.v1.Products.Attribute.Create
{
    public sealed record CreateAttributeRequest(
        string Code,
        string Name,
        string Description,
        string DataType,
        bool IsVariant,
        bool IsFilterable,
        bool IsRequired,
        int DisplayOrder,
        bool IsActive);
}
