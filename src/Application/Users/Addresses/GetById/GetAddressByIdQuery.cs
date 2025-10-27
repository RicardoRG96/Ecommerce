using Application.Abstractions.Messaging;

namespace Application.Users.Addresses.GetById
{
    public sealed record GetAddressByIdQuery(long AddressId) : IQuery<AddressResponse>;
}
