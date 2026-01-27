namespace Web.Api.Endpoints.v1.Products.Attribute.Create
{
    public sealed class CreateAttributeRequest
    {
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
