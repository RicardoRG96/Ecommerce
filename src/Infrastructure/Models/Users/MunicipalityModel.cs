using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Users
{
    public sealed class MunicipalityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MunicipalityId { get; set; }

        [Required]
        public long RegionId { get; set; }
        
        [Required]
        public string? Name { get; set; }

        [ForeignKey("RegionId")]
        public RegionModel? Region { get; set; }
    }
}
