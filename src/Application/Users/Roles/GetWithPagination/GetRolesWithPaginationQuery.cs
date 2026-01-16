using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Roles.GetWithPagination
{
    public sealed record GetRolesWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<RoleResponse>>;
}
