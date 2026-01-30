using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Brand : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<DiscountBrand> DiscountBrands { get; set; } = new List<DiscountBrand>();

        public static Brand Create(
            string name,
            string? description = null,
            string? logoUrl = null,
            string? bannerUrl = null,
            string? websiteUrl = null,
            bool isActive = true,
            bool isFeatured = false,
            int displayOrder = 0,
            string? metaTitle = null,
            string? metaDescription = null,
            string? metaKeywords = null)
        {
            return new Brand
            {
                Name = name,
                Slug = SlugGenerator.GenerateSlug(name),
                Description = description,
                LogoUrl = logoUrl,
                BannerUrl = bannerUrl,
                WebsiteUrl = websiteUrl,
                IsActive = isActive,
                IsFeatured = isFeatured,
                DisplayOrder = displayOrder,
                MetaTitle = metaTitle,
                MetaDescription = metaDescription,
                MetaKeywords = metaKeywords
            };
        }
    }
}
