using SharedKernel;

namespace Domain.Entities.Users
{
    public sealed class Address : BaseAuditableEntity
    {
        public long AddressId { get; set; }
        public long CountryId { get; set; } = 1; // Default to Chile
        public long MunicipalityId { get; set; }
        public string? Title { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Apartament { get; set; }
        public string? Reference { get; set; }
        public string? PostalCode { get; set; }
        public IList<User> Users { get; private set; } = new List<User>();
        public Country Country { get; set; } = null!;
        public Municipality Municipality { get; set; } = null!;
    }
}
