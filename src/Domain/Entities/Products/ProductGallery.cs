using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class ProductGallery : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long? ProductSkuId { get; set; }
        public string? MediaUrl { get; set; }
        public string? MediaType { get; set; }
        public string? MimeType { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public string? AltText { get; set; }
        public Product Product { get; set; } = null!;
        public ProductSku ProductSku { get; set; } = null!;
        public bool SkuBelongsToProduct => ProductSku != null && ProductSku.ProductId == ProductId;

        public static ProductGallery Create(
            long productId,
            long? productSkuId,
            string? mediaUrl,
            string? mediaType,
            string? mimeType,
            bool isPrimary,
            int displayOrder,
            string? altText )
        {
            return new ProductGallery
            {
                ProductId = productId,
                ProductSkuId = productSkuId,
                MediaUrl = mediaUrl,
                MediaType = mediaType,
                MimeType = mimeType,
                IsPrimary = isPrimary,
                DisplayOrder = displayOrder,
                AltText = altText
            };
        }

        public void SetPrimary()
        {
            IsPrimary = true;
        }
    }
}
