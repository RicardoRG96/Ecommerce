using SharedKernel;

namespace Domain.Users.Entities
{
    public sealed class User : BaseAuditableEntity
    {
        public long UserId { get; set; }
        public string? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public IList<Address> Addresses { get; private set; } = new List<Address>();
        public bool HasLegalAge() => (DateTime.UtcNow - DateOfBirth).TotalDays / 365.25 >= 18; // Legal age in Chile is 18
    }
}
