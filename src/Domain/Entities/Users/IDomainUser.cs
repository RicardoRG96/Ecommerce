namespace Domain.Entities.Users
{
    public interface IDomainUser
    {
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<Address> Addresses { get; set; }
        public bool HasLegalAge() => (DateTime.UtcNow - DateOfBirth).TotalDays / 365.25 >= 18; // Legal age in Chile is 18
    }
}
