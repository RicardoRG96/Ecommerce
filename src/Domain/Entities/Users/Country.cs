using SharedKernel;

namespace Domain.Entities.Users
{
    public sealed class Country : BaseAuditableEntity
    {
        public long CountryId { get; set; }
        public string? Name { get; set; }
        public IList<Address> Addresses { get; private set; } = new List<Address>();
    }
}
