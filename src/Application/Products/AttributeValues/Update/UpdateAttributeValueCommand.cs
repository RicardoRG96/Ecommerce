using Application.Abstractions.Messaging;

namespace Application.Products.AttributeValues.Update
{
    public sealed record UpdateAttributeValueCommand(
        long Id,
        long AttributeId,
        string Value,
        decimal? NumericValue,
        bool? BooleanValue,
        int DisplayOrder) : ICommand;
}
