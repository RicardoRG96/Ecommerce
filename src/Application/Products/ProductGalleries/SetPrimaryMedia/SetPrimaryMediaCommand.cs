using Application.Abstractions.Messaging;

namespace Application.Products.ProductGalleries.SetPrimaryMedia
{
    public sealed record SetPrimaryMediaCommand(
        long Id,
        long ProductId) : ICommand;
}
