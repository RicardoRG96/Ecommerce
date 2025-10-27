using Application.Abstractions.Messaging;

namespace Application.Users.Addresses.Delete
{
    public sealed record DeleteAddressCommand(long AddressId) : ICommand;
}
