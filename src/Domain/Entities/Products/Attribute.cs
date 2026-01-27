using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Attribute : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DataType { get; set; }
        public bool IsVariant { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();

        public static Attribute Create(
            string code,
            string name,
            string description,
            string dataType,
            bool isVariant,
            bool isFilterable,
            bool isRequired,
            int displayOrder,
            bool isActive)
        {
            return new Attribute
            {
                Code = code,
                Name = name,
                Description = description,
                DataType = dataType,
                IsVariant = isVariant,
                IsFilterable = isFilterable,
                IsRequired = isRequired,
                DisplayOrder = displayOrder,
                IsActive = isActive
            };
        }

        public void Update(
            string code,
            string name,
            string description,
            string dataType,
            bool isVariant,
            bool isFilterable,
            bool isRequired,
            int displayOrder)
        {
            Code = code;
            Name = name;
            Description = description;
            DataType = dataType;
            IsVariant = isVariant;
            IsFilterable = isFilterable;
            IsRequired = isRequired;
            DisplayOrder = displayOrder;
        }
    }
}
