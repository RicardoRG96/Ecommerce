using Application.Abstractions.Messaging;

namespace Application.Products.Attributes.Activate
{
    public sealed record ActivateAttributeCommand(long AttributeId) : ICommand;
}
