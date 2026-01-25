using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Products.Products.GetAllProductsWithPagination
{
    public sealed record GetProductsWithPaginationQuery(
        int PageNumber,
        int PageSize) : IQuery<PaginatedList<ProductResponse>>;
}
