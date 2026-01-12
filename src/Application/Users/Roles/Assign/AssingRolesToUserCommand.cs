using Application.Abstractions.Messaging;

namespace Application.Users.Roles.Assign
{
    public sealed record AssingRolesToUserCommand(
        List<Role> Roles,
        long UserId) : ICommand;
}
