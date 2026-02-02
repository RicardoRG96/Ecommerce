using Application.Abstractions.Messaging;

namespace Application.Products.ProductAttributeValues.Remove
{
    public sealed record RemoveAttributeValueFromSkuCommand(
        long SkuId,
        long AttributeValueId) : ICommand;
}
