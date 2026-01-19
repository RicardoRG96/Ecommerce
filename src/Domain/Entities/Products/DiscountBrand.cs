namespace Domain.Entities.Products
{
    public sealed class DiscountBrand
    {
        public long DiscountId { get; set; }
        public long BrandId { get; set; }
        public Discount Discount { get; set; } = null!;
        public Brand Brand { get; set; } = null!;
    }
}
