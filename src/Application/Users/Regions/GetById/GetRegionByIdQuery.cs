using Application.Abstractions.Messaging;

namespace Application.Users.Regions.GetById
{
    public sealed record GetRegionByIdQuery(long RegionId) : IQuery<RegionResponse>;
}
