using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Category : BaseAuditableEntity
    {
        public long CategoryId { get; set; }
        public long ParentId { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public bool IsVisibleInMenu { get; set; }
        public int DisplayOrder { get; set; }
        public string? ImageUrl { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public Category? Parent { get; set; } = null!;
        public ICollection<Category> Children { get; set; } = [];
        public ICollection<DiscountCategory> DiscountCategory { get; set; } = new List<DiscountCategory>();
    }
}
