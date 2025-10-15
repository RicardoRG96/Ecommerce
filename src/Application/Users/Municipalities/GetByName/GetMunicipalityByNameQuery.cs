using Application.Abstractions.Messaging;

namespace Application.Users.Municipalities.GetByName
{
    public sealed record GetMunicipalityByNameQuery(string Name) : IQuery<MunicipalityResponse>;
}
