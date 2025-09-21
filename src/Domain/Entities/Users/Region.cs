using SharedKernel;

namespace Domain.Entities.Users
{
    public sealed class Region : BaseAuditableEntity
    {
        public long RegionId { get; set; }
        public string? Name { get; set; }
        public IList<Municipality> Municipalities { get; private set; } = new List<Municipality>();
    }
}
