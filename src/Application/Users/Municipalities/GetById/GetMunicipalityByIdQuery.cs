using Application.Abstractions.Messaging;

namespace Application.Users.Municipalities.GetById
{
    public sealed record GetMunicipalityByIdQuery : IQuery<MunicipalityResponse>;
}
