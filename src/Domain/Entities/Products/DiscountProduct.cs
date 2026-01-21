namespace Domain.Entities.Products
{
    public sealed class DiscountProduct
    {
        public long DiscountId { get; set; }
        public long ProductId { get; set; }
        public Discount Discount { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
