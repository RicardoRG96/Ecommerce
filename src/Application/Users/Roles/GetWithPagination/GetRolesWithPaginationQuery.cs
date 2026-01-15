using Application.Abstractions.Messaging;

namespace Application.Users.Roles.GetWithPagination
{
    public sealed record GetRolesWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<RoleResponse>;
}
