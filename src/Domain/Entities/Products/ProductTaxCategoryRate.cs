namespace Domain.Entities.Products
{
    public sealed class ProductTaxCategoryRate
    {
        public long ProductTaxCategoryId { get; set; }
        public long TaxRateId { get; set; }
        public ProductTaxCategory ProductTaxCategory { get; set; } = null!;
        public TaxRate TaxRate { get; set; } = null!;
    }
}
