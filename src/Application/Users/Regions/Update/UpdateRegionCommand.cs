using Application.Abstractions.Messaging;

namespace Application.Users.Regions.Update
{
    public sealed record UpdateRegionCommand(long RegionId, string Name) : ICommand;
}
