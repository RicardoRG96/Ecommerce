using Application.Abstractions.Messaging;

namespace Application.Users.Roles.GetById
{
    public sealed record GetRoleByIdQuery(long Id) : IQuery<RoleResponse>;
}
