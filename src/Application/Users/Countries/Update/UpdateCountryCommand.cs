using Application.Abstractions.Messaging;

namespace Application.Users.Countries.Update
{
    public sealed record UpdateCountryCommand(long Id, string Name) : ICommand;
}
