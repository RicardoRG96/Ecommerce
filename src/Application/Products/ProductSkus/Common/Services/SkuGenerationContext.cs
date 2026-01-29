namespace Application.Products.ProductSkus.Common.Services
{
    public sealed class SkuGenerationContext
    {
        public long ProductSkuId { get; set; }
        public string CategoryCode { get; set; } = default!;
        public string BrandCode { get; init; } = default!;
        public IReadOnlyDictionary<string, string>? Variants { get; init; }
    }
}