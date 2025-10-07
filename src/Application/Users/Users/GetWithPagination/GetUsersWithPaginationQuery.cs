using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Users.GetWithPagination
{
    public sealed record GetUsersWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<UserResponse>>;
}
