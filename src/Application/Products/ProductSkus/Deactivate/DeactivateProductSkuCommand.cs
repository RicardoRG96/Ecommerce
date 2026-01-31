using Application.Abstractions.Messaging;

namespace Application.Products.ProductSkus.Deactivate
{
    public sealed record DeactivateProductSkuCommand(long Id) : ICommand;
}
