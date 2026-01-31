using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Category : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
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
        public ICollection<DiscountCategory> DiscountCategories { get; set; } = new List<DiscountCategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public static Category Create(
            long? parentId,
            string name,
            string? description = null,
            string? imageUrl = null,
            string? icon = null,
            bool isActive = true,
            bool isVisibleInMenu = true,
            int displayOrder = 0,
            string? metaTitle = null,
            string? metaDescription = null,
            string? metaKeywords = null)
        {
            return new Category
            {
                ParentId = parentId,
                Name = name,
                Slug = SlugGenerator.GenerateSlug(name),
                Description = description,
                ImageUrl = imageUrl,
                Icon = icon,
                IsActive = isActive,
                IsVisibleInMenu = isVisibleInMenu,
                DisplayOrder = displayOrder,
                MetaTitle = metaTitle,
                MetaDescription = metaDescription,
                MetaKeywords = metaKeywords
            };
        }
    }
}
