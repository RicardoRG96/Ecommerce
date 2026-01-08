using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<long>, IDomainUser
    {
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool HasLegalAge() => (DateTime.UtcNow - DateOfBirth).TotalDays / 365.25 >= 18; // Legal age in Chile is 18
    }
}
