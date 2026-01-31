namespace Application.Products.ProductSkus.Common.Services
{
    public interface ISkuGenerationContextBuilder
    {
        Task<SkuGenerationContext> BuildForProductAsync(
            long productId, 
            CancellationToken cancellationToken);
    }
}