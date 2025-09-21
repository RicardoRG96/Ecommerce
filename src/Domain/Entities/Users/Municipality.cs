using SharedKernel;

namespace Domain.Entities.Users
{
    public sealed class Municipality : BaseAuditableEntity
    {
        public long MunicipalityId { get; set; }
        public long RegionId { get; set; }
        public string? Name { get; set; }
        public Region Region { get; set; } = null!;
        public IList<Address> Addresses { get; private set; } = new List<Address>();
    }
}
