namespace Web.Api.Endpoints.v1.Products.AttributeValue.Update
{
    public sealed record UpdateAttributeValueRequest(
        long AttributeId,
        string Value,
        decimal? NumericValue,
        bool? BooleanValue,
        int DisplayOrder);
}
