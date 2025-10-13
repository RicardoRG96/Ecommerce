using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Regions.GetWithPagination
{
    public sealed record GetRegionsWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<RegionResponse>>;
}
