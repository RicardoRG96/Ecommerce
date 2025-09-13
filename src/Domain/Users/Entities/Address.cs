namespace Domain.Users.Entities
{
    public sealed class Address
    {
        public long AddressId { get; set; }
        public long UserId { get; set; }
        public long CountryId { get; set; }
        public long MunicipalityId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? Apartament { get; set; }
        public string? Reference { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
