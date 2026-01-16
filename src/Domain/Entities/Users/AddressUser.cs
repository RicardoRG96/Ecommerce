using SharedKernel;

namespace Domain.Entities.Users
{
    public sealed class AddressUser : BaseAuditableEntity
    {
        public long AddressId { get; set; }
        public long ApplicationUserId { get; set; }
        public bool IsDefault { get; set; }
        public Address Address { get; set; } = null!;
    }
}
