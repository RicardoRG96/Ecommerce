using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Products.Products.GetAllPublishedProductsWithPagination
{
    public sealed record GetAllPublishedProductsWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<PublishedProductResponse>>;
}
