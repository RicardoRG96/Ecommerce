using Application.Users.Countries.GetById;
using Application.Users.Municipalities.GetById;

namespace Application.Users.Addresses.GetById
{
    public sealed class AddressResponse
    {
        public long Id { get; set; }
        public CountryResponse Country { get; set; }
        public MunicipalityResponse Municipality { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Apartament { get; set; }
        public string Reference { get; set; }
        public string PostalCode { get; set; }
    }
}
