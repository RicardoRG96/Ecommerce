using Application.Abstractions.Messaging;

namespace Application.Users.Countries.Create
{
    public sealed record CreateCountryCommand(string Name) : ICommand<long>;
}
