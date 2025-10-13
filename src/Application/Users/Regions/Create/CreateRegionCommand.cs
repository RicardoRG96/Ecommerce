using Application.Abstractions.Messaging;

namespace Application.Users.Regions.Create
{
    public sealed record CreateRegionCommand(string Name) : ICommand<long>;
}
