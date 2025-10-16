using Application.Abstractions.Messaging;

namespace Application.Users.Municipalities.Create
{
    public sealed class CreateMunicipalityCommand : ICommand<long>
    {
        public long RegionId { get; set; }
        public string Name { get; set; }
    }
}
