namespace Web.Api.Endpoints.v1.Products.AttributeValue.Create
{
    public sealed record CreateAttributeValueRequest(
        long AttributeId,
        string Value,
        decimal? NumericValue,
        bool? BooleanValue,
        int DisplayOrder,
        bool IsActive);
}
