using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Category : BaseAuditableEntity
    {
        public long CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
