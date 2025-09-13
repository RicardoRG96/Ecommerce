namespace Domain.Users.Entities
{
    public sealed class Municipality
    {
        public long MunicipalityId { get; set; }
        public long RegionId { get; set; }
        public string Name { get; set; }
    }
}
