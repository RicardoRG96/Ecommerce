using Application.Abstractions.Messaging;

namespace Application.Users.Users.Update
{
    public sealed class UpdateUserCommand : ICommand
    {
        public long UserId { get; set; }
        public string? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
