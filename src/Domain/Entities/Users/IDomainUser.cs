namespace Domain.Entities.Users
{
    public interface IDomainUser
    {
        public long Id { get; set; }
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public IList<Address> Addresses { get; set; }
        public bool HasLegalAge();
    }
}
