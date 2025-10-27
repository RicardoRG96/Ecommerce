using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Addresses.GetWithPagination
{
    public sealed record GetAddressesWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<AddressResponse>>;
}
