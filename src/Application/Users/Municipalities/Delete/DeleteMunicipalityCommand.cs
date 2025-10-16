using Application.Abstractions.Messaging;

namespace Application.Users.Municipalities.Delete
{
    public sealed record DeleteMunicipalityCommand(long MunicipalityId) : ICommand;
}
