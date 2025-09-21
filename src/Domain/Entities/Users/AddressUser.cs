using SharedKernel;

namespace Domain.Entities.Users
{
    public sealed class AddressUser : BaseAuditableEntity
    {
        public long AddressId { get; set; }
        public long UserId { get; set; }
        public bool IsDefault { get; set; }
    }
}
