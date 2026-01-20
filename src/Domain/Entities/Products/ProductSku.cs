using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class ProductSku : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string? SkuCode { get; set; }
        public string? BarCode { get; set; }
        public decimal Price { get; set; }
        public decimal ComparedAtPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public Product Product { get; set; } = null!;
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
        public ICollection<DiscountSku> DiscountSku { get; set; } = new List<DiscountSku>();
    }
}
