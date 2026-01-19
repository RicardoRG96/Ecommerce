using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class TaxRate : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public char CountryCode { get; set; }
        public string? TaxType { get; set; }
        public decimal Rate { get; set; }
        public bool IsCompound { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
