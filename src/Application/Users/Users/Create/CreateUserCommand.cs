using Application.Abstractions.Messaging;

namespace Application.Users.Users.Create
{
    public sealed record CreateUserCommand(
        string Avatar,
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password,
        DateTime DateOfBirth,
        string PhoneNumber) : ICommand<long>;
}
