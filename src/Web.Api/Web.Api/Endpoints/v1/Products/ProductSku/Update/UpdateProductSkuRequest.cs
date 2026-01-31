namespace Web.Api.Endpoints.v1.Products.ProductSku.Update
{
    public sealed record UpdateProductSkuRequest(
        long ProductId,
        string? BarCode,
        decimal Price,
        decimal? Cost,
        decimal? Weight,
        decimal? Length,
        decimal? Width,
        decimal? Height,
        int DisplayOrder);
}
