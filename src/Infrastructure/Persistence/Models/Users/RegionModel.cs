namespace Infrastructure.Persistence.Models.Users
{
    public sealed class RegionModel
    {
        public long RegionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<MunicipalityModel> Municipalities { get; } = new List<MunicipalityModel>();
    }
}
