using Application.Abstractions.Messaging;

namespace Application.Products.Attributes.Deactivate
{
    public sealed record DeactivateAttributeCommand(long AttributeId) : ICommand;
}
