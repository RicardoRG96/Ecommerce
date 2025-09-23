namespace Application.Users.User.GetById
{
    public sealed class UserResponse
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
