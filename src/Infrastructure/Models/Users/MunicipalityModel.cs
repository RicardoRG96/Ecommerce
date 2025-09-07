using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Users
{
    public sealed class MunicipalityModel
    {
        public long MunicipalityId { get; set; }
        public long RegionId { get; set; }
        public RegionModel Region { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public ICollection<AddressModel> Addresses { get; } = new List<AddressModel>();
    }
}
