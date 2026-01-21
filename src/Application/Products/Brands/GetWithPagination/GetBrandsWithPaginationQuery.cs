using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Products.Brands.GetWithPagination
{
    public sealed record GetBrandsWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<BrandResponse>>;
}
