using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class AttributeValue : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long AttributeId { get; set; }
        public string? Value { get; set; }
        public string? NormalizedValue { get; set; }
        public decimal? NumericValue { get; set; }
        public bool? BooleanValue { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public Attribute Attribute { get; set; } = null!;
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();

        public static AttributeValue Create(
            long attributeId,
            string value,
            decimal? numericValue = null,
            bool? booleanValue = null,
            int displayOrder = 0,
            bool isActive = true)
        {
            return new AttributeValue
            {
                AttributeId = attributeId,
                Value = value,
                NormalizedValue = value.ToLowerInvariant(),
                NumericValue = numericValue,
                BooleanValue = booleanValue,
                DisplayOrder = displayOrder,
                IsActive = isActive
            };
        }

        public void Update(
            long attributeId,
            string value,
            decimal? numericValue = null,
            bool? booleanValue = null,
            int displayOrder = 0)
        {
            AttributeId = attributeId;
            Value = value;
            NormalizedValue = value.ToLowerInvariant();
            NumericValue = numericValue;
            BooleanValue = booleanValue;
            DisplayOrder = displayOrder;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
