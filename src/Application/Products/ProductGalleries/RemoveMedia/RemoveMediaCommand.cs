using Application.Abstractions.Messaging;

namespace Application.Products.ProductGalleries.RemoveMedia
{
    public sealed record RemoveMediaCommand(long Id) : ICommand;
}
