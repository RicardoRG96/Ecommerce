namespace Application.Products.ProductAttributeValues.Common.Mappers
{
    public sealed class ProductAttributeValueResponse
    {
        public ProductSkuResponse? ProductSku { get; set; }
        public string? AttributeCode { get; set; }
        public string? Value { get; set; }
        public string? Name { get; set; }
        public string? DataType { get; set; }
        public bool IsVariant { get; set; }
    }

    public sealed class ProductSkuResponse
    {
        public long Id { get; set; }
        public ProductResponse? Product { get; set; }
        public string? SkuCode { get; set; }
        public string? BarCode { get; set; }
        public decimal Price { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
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
}
