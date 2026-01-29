namespace Application.Products.AttributeValues.Common.Mappers
{
    public sealed class AttributeValueResponse
    {
        public long Id { get; set; }
        public AttributeResponse? Attribute { get; set; }
        public string? Value { get; set; }
        public decimal? NumericValue { get; set; }
        public bool? BooleanValue { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public sealed class AttributeResponse
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
    }
}