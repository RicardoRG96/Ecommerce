namespace Domain.Entities.Products
{
    public sealed class DiscountSku
    {
        public long DiscountId { get; set; }
        public long ProductSkuId { get; set; }
        public Discount Discount { get; set; } = null!;
        public ProductSku ProductSku { get; set; } = null!;
    }
}
