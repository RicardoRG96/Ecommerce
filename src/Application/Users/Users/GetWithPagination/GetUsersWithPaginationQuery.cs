using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Users.Get
{
    public sealed record GetUsersWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<UserResponse>>;
}
