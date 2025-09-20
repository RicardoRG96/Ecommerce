using SharedKernel;

namespace Domain.Users.Entities
{
    public sealed class AddressUser : BaseAuditableEntity
    {
        public long AddressId { get; set; }
        public long UserId { get; set; }
        public bool IsDefault { get; set; }
    }
}
