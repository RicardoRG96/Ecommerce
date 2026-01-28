namespace Web.Api.Endpoints.v1.Products.Attribute.Update
{
    public sealed record UpdateAttributeRequest(
        string Code,
        string Name,
        string Description,
        string DataType,
        bool IsVariant,
        bool IsFilterable,
        bool IsRequired,
        int DisplayOrder);
}
