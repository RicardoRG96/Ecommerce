using Application.Abstractions.Messaging;

namespace Application.Products.ProductSkus.Update
{
    public sealed record UpdateProductSkuCommand(
        long Id,
        long ProductId,
        string? BarCode,
        decimal Price,
        decimal? Cost,
        decimal? Weight,
        decimal? Length,
        decimal? Width,
        decimal? Height,
        int DisplayOrder) : ICommand;
}
