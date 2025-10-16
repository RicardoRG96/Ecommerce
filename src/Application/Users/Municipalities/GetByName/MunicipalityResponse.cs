using Application.Users.Regions.GetByName;

namespace Application.Users.Municipalities.GetByName
{
    public sealed class MunicipalityResponse
    {
        public long Id { get; set; }
        public RegionResponse? Region { get; set; }
        public string Name { get; set; }
    }
}
