using Application.Abstractions.Messaging;

namespace Application.Users.Addresses.Create
{
    public sealed record CreateAddressCommand(
        long CountryId,
        long MunicipalityId,
        string Title,
        string City,
        string Street,
        string Number,
        string Apartament,
        string Reference,
        string PostalCode) : ICommand<long>;
}
