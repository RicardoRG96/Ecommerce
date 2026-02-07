using Application.Abstractions.Messaging;

namespace Application.Products.ProductGalleries.AddMedia
{
    public sealed record AddMediaCommand(
        Stream Stream,
        string FileName,
        string ContentType) : ICommand<long>;
}
