namespace Infrastructure.Models.Users
{
    public sealed class RegionModel
    {
        public long RegionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<MunicipalityModel>? Municipalities { get; } = new List<MunicipalityModel>();
    }
}
