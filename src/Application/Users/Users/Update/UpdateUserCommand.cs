using Application.Abstractions.Messaging;

namespace Application.Users.Users.Update
{
    public sealed record UpdateUserCommand(
        long UserId,
        string Avatar,
        string FirstName,
        string LastName,
        string PhoneNumber) : ICommand;
}
