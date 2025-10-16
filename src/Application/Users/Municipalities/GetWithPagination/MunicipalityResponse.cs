using Application.Users.Regions.GetWithPagination;

namespace Application.Users.Municipalities.GetWithPagination
{
    public sealed class MunicipalityResponse
    {
        public long Id { get; set; }
        public RegionResponse? Region { get; set; }
        public string Name { get; set; }
    }
}
