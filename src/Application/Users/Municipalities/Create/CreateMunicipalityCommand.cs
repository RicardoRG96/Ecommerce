using Application.Abstractions.Messaging;

namespace Application.Users.Municipalities.Create
{
    public sealed record CreateMunicipalityCommand(long RegionId, string Name) : ICommand<long>;
}
