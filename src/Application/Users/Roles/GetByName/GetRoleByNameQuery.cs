using Application.Abstractions.Messaging;

namespace Application.Users.Roles.GetByName
{
    public sealed record GetRoleByNameQuery(string Name) : IQuery<RoleResponse>;
}
