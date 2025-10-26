using Application.Abstractions.Messaging;

namespace Application.Users.Addresses.GetByTitle
{
    public sealed record GetAddressByTitleQuery(string title) : IQuery<AddressResponse>;
}
