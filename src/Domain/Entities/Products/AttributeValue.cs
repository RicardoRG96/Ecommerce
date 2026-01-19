using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class AttributeValue : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long AttributeId { get; set; }
        public string? Value { get; set; }
        public string? NormalizedValue { get; set; }
        public decimal NumericValue { get; set; }
        public bool BooleanValue { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public Attribute Attribute { get; set; } = null!;
        public ICollection<ProductSku> ProductSkus { get; set; } = new List<ProductSku>();
    }
}
