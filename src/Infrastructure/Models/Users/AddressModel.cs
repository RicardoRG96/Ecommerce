using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Users
{
    public sealed class AddressModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AddressId { get; set; }

        [Required]
        public long UserId { get; set; }

        public long CountryId { get; set; } = 1; // Default to Chile

        [Required]
        public long MunicipalityId { get; set; }
        public string? Title { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Street { get; set; }

        [Required]
        public string? Number { get; set; }
        public string? Apartament { get; set; }
        public string? Reference { get; set; }
        public string? PostalCode { get; set; }

        [Required]
        public bool IsDefault { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        [ForeignKey("CountryId")]
        public CountryModel? Countrty { get; set; }

        [ForeignKey("MunicipalityId")]
        public MunicipalityModel? Municipality { get; set; }
    }
}
