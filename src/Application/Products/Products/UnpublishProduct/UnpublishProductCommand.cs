using Application.Abstractions.Messaging;

namespace Application.Products.Products.UnpublishProduct
{
    public sealed record UnpublishProductCommand(long ProductId) : ICommand;
}
