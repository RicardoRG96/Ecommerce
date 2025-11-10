using Application.Abstractions.Messaging;

namespace Application.Users.Municipalities.Update
{
    public sealed record UpdateMunicipalityCommand(
        long MunicipalityId,
        long RegionId,
        string Name) : ICommand;
}
