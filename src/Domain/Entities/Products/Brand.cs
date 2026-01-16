using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Brand : BaseAuditableEntity
    {
        public long BrandId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}
