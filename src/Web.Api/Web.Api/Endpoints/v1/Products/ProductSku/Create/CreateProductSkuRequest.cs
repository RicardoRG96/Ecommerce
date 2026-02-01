namespace Web.Api.Endpoints.v1.Products.ProductSku.Create
{
    public sealed record CreateProductSkuRequest(
        long ProductId,
        string? BarCode,
        decimal Price,
        decimal? Cost,
        decimal? Weight,
        decimal? Length,
        decimal? Width,
        decimal? Height,
        bool IsActive,
        int DisplayOrder);
}
