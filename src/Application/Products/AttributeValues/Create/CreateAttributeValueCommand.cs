using Application.Abstractions.Messaging;

namespace Application.Products.AttributeValues.Create
{
    public sealed record CreateAttributeValueCommand(
        long AttributeId,
        string Value,
        decimal? NumericValue,
        bool? BooleanValue,
        int DisplayOrder,
        bool IsActive) : ICommand<long>;
}
