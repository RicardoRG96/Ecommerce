using Application.Abstractions.Messaging;

namespace Application.Products.ProductAttributeValues.Assign
{
    public sealed record AssignAttributeValueToSkuCommand(
        long SkuId,
        long AttributeValueId) : ICommand;
}
