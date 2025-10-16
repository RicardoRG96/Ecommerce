using Application.Abstractions.Messaging;

namespace Application.Users.Municipalities.Update
{
    public sealed class UpdateMunicipalityCommand : ICommand
    {
        public long MunicipalityId { get; set; }
        public long RegionId { get; set; }
        public string Name { get; set; }
    }
}
