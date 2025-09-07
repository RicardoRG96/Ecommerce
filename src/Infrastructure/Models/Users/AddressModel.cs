using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Users
{
    public sealed class AddressModel
    {
        public long AddressId { get; set; }

        public long UserId { get; set; }
        public UserModel User { get; set; } = null!;

        public long CountryId { get; set; } = 1; // Default to Chile
        public CountryModel Countrty { get; set; } = null!;

        public long MunicipalityId { get; set; }
        public MunicipalityModel Municipality { get; set; } = null!;
        public string Title { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string Number { get; set; } = string.Empty;
        public string? Apartament { get; set; }
        public string? Reference { get; set; }
        public string? PostalCode { get; set; }

        public bool IsDefault { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
