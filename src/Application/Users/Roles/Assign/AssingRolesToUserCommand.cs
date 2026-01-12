using Application.Abstractions.Messaging;
using Domain.Entities.Users;

namespace Application.Users.Roles.Assign
{
    public sealed record AssingRolesToUserCommand(
        List<Role> Roles,
        long UserId) : ICommand;
}
