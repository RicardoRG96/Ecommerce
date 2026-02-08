using Domain.Errors.Products;
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
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDigital { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public Brand? Brand { get; set; } = null!;
        public Category? Category { get; set; } = null!;
        public ProductTaxCategory? ProductTaxCategory { get; set; } = null!;
        public ICollection<DiscountProduct> DiscountProducts { get; set; } = new List<DiscountProduct>();
        public ICollection<ProductSku> ProductSkus { get; set; } = new List<ProductSku>();
        public ICollection<ProductGallery> ProductGalleries { get; set; } = new List<ProductGallery>();

        public void Publish()
        {
            IsPublished = true;
        }

        public void Unpublish()
        {
            IsPublished = false;
        }

        public void Deactivate()
        {
            IsActive = false;
            IsPublished = false;
        }

        public static Product Create(
            string name,
            string description,
            string shortDescription,
            long brandId,
            long categoryId,
            long productTaxCategoryId,
            bool isActive,
            bool isFeatured,
            bool isDigital,
            string metaTitle,
            string metaDescription,
            string metaKeywords)
        {
            return new Product
            {
                Name = name,
                Slug = SlugGenerator.GenerateSlug(name),
                Description = description,
                ShortDescription = shortDescription,
                BrandId = brandId,
                CategoryId = categoryId,
                ProductTaxCategoryId = productTaxCategoryId,
                IsActive = isActive,
                IsFeatured = isFeatured,
                IsDigital = isDigital,
                MetaTitle = metaTitle,
                MetaDescription = metaDescription,
                MetaKeywords = metaKeywords
            };
        }

        public void Update(
            string name,
            string description,
            string shortDescription,
            long brandId,
            long categoryId,
            long productTaxCategoryId,
            bool isFeatured,
            bool isDigital,
            string metaTitle,
            string metaDescription,
            string metaKeywords)
        {
            // Only regenerate slug if name changed
            if (Name != name)
            {
                Name = name;
                Slug = SlugGenerator.GenerateSlug(name);
            }

            Description = description;
            ShortDescription = shortDescription;
            BrandId = brandId;
            CategoryId = categoryId;
            ProductTaxCategoryId = productTaxCategoryId;
            IsFeatured = isFeatured;
            IsDigital = isDigital;
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            MetaKeywords = metaKeywords;
        }

        public Result ReorderGallery(IReadOnlyList<long> orderedIds)
        {
            if (orderedIds.Count != ProductGalleries.Count)
            {
                return Result.Failure(ProductErrors.InvalidOrderGallery);
            }

            if (orderedIds.Distinct().Count() != orderedIds.Count)
            {
                return Result.Failure(ProductErrors.DuplicateIdsInGallery);
            }

            Dictionary<long, ProductGallery> galleryMap = ProductGalleries.ToDictionary(g => g.Id);

            if (orderedIds.Any(id => !galleryMap.ContainsKey(id)))
            {
                return Result.Failure(ProductErrors.ForeignItemInGallery);
            }

            for (int i = 0; i < orderedIds.Count; i++)
            {
                galleryMap[orderedIds[i]].DisplayOrder = i + 1;
            }

            return Result.Success();
        }
    }
}
