using Application.Abstractions.Messaging;

namespace Application.Users.Regions.GetByName
{
    public sealed record GetByNameQuery(string Name) : IQuery<RegionResponse>;
}
