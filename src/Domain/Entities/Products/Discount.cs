using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class Discount : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DiscountType { get; set; }
        public decimal Value { get; set; }
        public decimal MaxDiscountAmount { get; set; }
        public bool IsStackable { get; set; }
        public int Priority { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public decimal MinOrderAmount { get; set; }
        public int UsageLimit { get; set; }
        public int UsageLimitPerUser { get; set; }
        public bool IsActive { get; set; }
    }
}
