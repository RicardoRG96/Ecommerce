using Application.Abstractions.Messaging;

namespace Application.Users.Regions.GetByName
{
    public sealed record GetRegionByNameQuery(string Name) : IQuery<RegionResponse>;
}
