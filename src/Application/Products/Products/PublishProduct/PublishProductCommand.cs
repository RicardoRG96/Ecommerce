using Application.Abstractions.Messaging;

namespace Application.Products.Products.PublishProduct
{
    public sealed record PublishProductCommand(
        long ProductId, 
        bool IsActive) : ICommand;
}
