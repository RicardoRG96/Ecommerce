using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class DiscountCode : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public long DiscountId { get; set; }
        public bool IsSingleUse { get; set; }
        public bool IsActive { get; set; }
        public int UsageLimit { get; set; }
        public int UsageLimitPerUser { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public Discount Discount { get; set; } = null!;
    }
}
