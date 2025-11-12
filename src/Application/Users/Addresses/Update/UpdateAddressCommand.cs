using Application.Abstractions.Messaging;

namespace Application.Users.Addresses.Update
{
    public sealed record UpdateAddressCommand(
        long AddressId,
        long MunicipalityId,
        string Title,
        string City,
        string Street,
        string Number,
        string Apartament,
        string Reference,
        string PostalCode) : ICommand;
}
