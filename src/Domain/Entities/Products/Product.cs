using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Product : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public long BrandId { get; set; }
        public long CategoryId { get; set; }
        public long ProductTaxCategoryId { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDigital { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public Brand? Brand { get; set; } = null!;
        public Category? Category { get; set; } = null!;
        public ProductTaxCategory? ProductTaxCategory { get; set; } = null!;
    }
}
