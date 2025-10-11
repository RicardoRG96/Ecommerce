using Application.Abstractions.Messaging;

namespace Application.Users.Countries.Delete
{
    public sealed record DeleteCountryCommand(long Id) : ICommand;
}
