using Application.Abstractions.Messaging;

namespace Application.Users.Regions.Delete
{
    public sealed record DeleteRegionCommand(long RegionId) : ICommand;
}
