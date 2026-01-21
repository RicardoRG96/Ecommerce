namespace Domain.Entities.Products
{
    public sealed class DiscountCategory
    {
        public long DiscountId { get; set; }
        public long CategoryId { get; set; }
        public Discount Discount { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
