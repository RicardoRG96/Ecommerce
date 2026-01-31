namespace Application.Products.ProductSkus.Common.Mappers
{
    public sealed class ProductSkuResponse
    {
        public long Id { get; set; }
        public ProductResponse? ProductResponse { get; set; }
        public string? SkuCode { get; set; }
        public string? BarCode { get; set; }
        public decimal Price { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public List<ProductAttributeValueResponse>? ProductAttributeValues { get; set; } = [];
        public List<ProductGalleryResponse>? ProductGalleriesResponse { get; set; } = [];
        public ProductSkuDiscountsResponse? ProductSkuDiscountsResponse { get; set; }
        public ProductSkuStockResponse? ProductSkuStockResponse { get; set; }
    }

    public sealed class ProductResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public BrandResponse? Brand { get; set; }
        public CategoryResponse? Category { get; set; }
    }

    public sealed class BrandResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? LogoUrl { get; set; }
    }

    public sealed class CategoryResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
    }

    public sealed class ProductAttributeValueResponse
    {
        public string? Value { get; set; }
        public string? Name { get; set; }
        public bool IsVariant { get; set; }
    }

    public sealed class ProductGalleryResponse
    {
        public string? MediaUrl { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public string? AltText { get; set; }
    }

    public sealed class ProductSkuDiscountsResponse
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? DiscountType { get; set; }
        public decimal Value { get; set; }
        public decimal MaxDiscountAmount { get; set; }
        public bool IsStackable { get; set; }
        public int Priority { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public decimal MinOrderAmount { get; set; }
        public int UsageLimit { get; set; }
        public bool IsActive { get; set; }
    }

    public sealed class ProductSkuStockResponse
    {
        public long Id { get; set; }
        public long WarehouseId { get; set; }
        public int Stock { get; set; }
        public int ReservedStock { get; set; }
        public int MinStock { get; set; }
        public bool IsActive { get; set; }
    }
}
