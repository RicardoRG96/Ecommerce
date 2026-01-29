using Application.Abstractions.Messaging;

namespace Application.Products.AttributeValues.Activate
{
    public sealed record ActivateAttributeValueCommand(long Id) : ICommand;
}
