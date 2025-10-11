using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Countries.GetWithPagination
{
    public sealed record GetCountriesWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<CountryResponse>>;
}
