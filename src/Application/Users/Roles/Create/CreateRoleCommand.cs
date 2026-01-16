using Application.Abstractions.Messaging;

namespace Application.Users.Roles.Create
{
    public sealed record CreateRoleCommand(string Name) : ICommand<long>;
}
