using Application.Abstractions.Messaging;

namespace Application.Products.ProductGalleries.ReorderGallery
{
    public sealed record ReorderGalleryCommand(
        long ProductId,
        long SkuId,
        long[] OrderedGalleryItemIds) : ICommand;
}
