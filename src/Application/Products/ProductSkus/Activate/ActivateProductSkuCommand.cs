using Application.Abstractions.Messaging;

namespace Application.Products.ProductSkus.Activate
{
    public sealed record ActivateProductSkuCommand(long Id) : ICommand;
}
