using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Users
{
    public sealed class AddressModel
    {
        public long AddressId { get; set; }

        public long UserId { get; set; }
        public UserModel? User { get; set; }

        public long CountryId { get; set; } = 1; // Default to Chile
        public CountryModel? Countrty { get; set; }

        public long MunicipalityId { get; set; }
        public MunicipalityModel? Municipality { get; set; }
        public string? Title { get; set; }

        public string? City { get; set; }

        public string? Street { get; set; }

        public string? Number { get; set; }
        public string? Apartament { get; set; }
        public string? Reference { get; set; }
        public string? PostalCode { get; set; }

        public bool IsDefault { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
