using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Municipalities.GetWithPagination
{
    public sealed record GetMunicipalitiesWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<MunicipalityResponse>>;
}
