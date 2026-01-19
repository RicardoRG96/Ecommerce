using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class ProductAttributeValue : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long ProductSkuId { get; set; }
        public long AttributeId { get; set; }
        public long AttributeValueId { get; set; }
        public Attribute Attribute { get; set; } = null!;
        public ProductSku ProductSku { get; set; } = null!;
        public AttributeValue AttributeValue { get; set; } = null!;
    }
}
