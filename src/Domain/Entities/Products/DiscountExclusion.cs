namespace Domain.Entities.Products
{
    public sealed class DiscountExclusion
    {
        public long DiscountId { get; set; }
        public long ExcludedDiscountId { get; set; }
        public Discount Discount { get; set; } = null!;
    }
}
