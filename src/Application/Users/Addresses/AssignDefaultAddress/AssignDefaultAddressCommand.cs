using Application.Abstractions.Messaging;

namespace Application.Users.Addresses.AssignDefaultAddress
{
    public sealed record AssignDefaultAddressCommand(
        long UserId,
        long AddressId) : ICommand;
}
