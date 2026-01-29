using Application.Abstractions.Messaging;

namespace Application.Products.ProductSkus.Create
{
    public sealed record CreateProductSkuCommand(
        long ProductId,
        string? BarCode,
        decimal Price,
        decimal? Cost,
        decimal? Weight,
        decimal? Length,
        decimal? Width,
        decimal? Height,
        bool IsActive,
        int DisplayOrder) : ICommand<long>;
}
