using Application.Abstractions.Messaging;

namespace Application.Products.AttributeValues.Deactivate
{
    public sealed record DeactivateAttributeValueCommand(long Id) : ICommand;
}
