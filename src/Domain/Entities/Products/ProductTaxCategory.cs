using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class ProductTaxCategory : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductTaxCategoryRate> ProductTaxCategoryRates { get; set; } = new List<ProductTaxCategoryRate>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
