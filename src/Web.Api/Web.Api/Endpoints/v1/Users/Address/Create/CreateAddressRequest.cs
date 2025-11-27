namespace Web.Api.Endpoints.v1.Users.Address.Create
{
    public record CreateAddressRequest(
        long CountryId,
        long MunicipalityId,
        string Title,
        string City,
        string Street,
        string Number,
        string Apartament,
        string Reference,
        string PostalCode);
}
