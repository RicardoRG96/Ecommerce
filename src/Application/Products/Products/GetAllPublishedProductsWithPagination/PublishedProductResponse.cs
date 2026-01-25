namespace Application.Products.Products.GetAllPublishedProductsWithPagination
{
    public sealed class PublishedProductResponse
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
        public List<ProductSkuResponse> ProductSkus { get; set; } = [];
        public List<ProductGalleryResponse> ProductGalleries { get; set; } = [];

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

    public sealed class ProductSkuResponse
    {
        public long Id { get; set; }
        public string? SkuCode { get; set; }
        public decimal Price { get; set; }
        public decimal ComparedAtPrice { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public List<ProductGalleryResponse>? ProductGalleriesResponse { get; set; } = [];
    }

    public sealed class ProductGalleryResponse
    {
        public string? MediaUrl { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public string? AltText { get; set; }
    }
}
