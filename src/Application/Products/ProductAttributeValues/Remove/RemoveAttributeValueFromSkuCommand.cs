using Application.Abstractions.Messaging;

namespace Application.Products.ProductAttributeValues.Remove
{
    public sealed record RemoveAttributeValueFromSkuCommand(
        long ProductSkuId,
        long AttributeValueId) : ICommand;
}
