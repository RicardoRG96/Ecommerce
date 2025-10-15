using Application.Users.Regions.GetById;

namespace Application.Users.Municipalities.GetById
{
    public sealed class MunicipalityResponse
    {
        public long Id { get; set; }
        public RegionResponse? Region { get; set; }
        public string Name { get; set; }
    }
}
