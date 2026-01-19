using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Warehouse : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public char CountryCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
