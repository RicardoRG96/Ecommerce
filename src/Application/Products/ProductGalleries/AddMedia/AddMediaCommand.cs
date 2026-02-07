using Application.Abstractions.Messaging;

namespace Application.Products.ProductGalleries.AddMedia
{
    public sealed record AddMediaCommand(
        long ProductId,
        long? SkuId,
        Stream Stream,
        string FileName,
        string ContentType,
        bool IsPrimary,
        int DisplayOrder,
        string? AltText) : ICommand<long>;
}
