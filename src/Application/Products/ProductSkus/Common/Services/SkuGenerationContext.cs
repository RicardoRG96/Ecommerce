namespace Application.Products.ProductSkus.Common.Services
{
    public sealed class SkuGenerationContext
    {
        public long ProductSkuId { get; init; }
        public long ProductId { get; init; }
        public string CategoryCode { get; init; } = string.Empty;
        public string BrandCode { get; init; } = string.Empty;
        public string? ProductCode { get; init; }
        public IReadOnlyDictionary<string, string>? Variants { get; init; }
        public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
    }
}