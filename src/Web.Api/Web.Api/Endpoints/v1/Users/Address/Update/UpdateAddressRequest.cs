namespace Web.Api.Endpoints.v1.Users.Address.Update
{
    public record UpdateAddressRequest(
        long MunicipalityId,
        string Title,
        string City,
        string Street,
        string Number,
        string Apartament,
        string Reference,
        string PostalCode);
}
