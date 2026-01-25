using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Products.GetAllPublishedProductsWithPagination
{
    internal sealed class GetAllPublishedProductsWithPaginationQueryHandler
        : IQueryHandler<GetAllPublishedProductsWithPaginationQuery, PaginatedList<PublishedProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllPublishedProductsWithPaginationQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<PaginatedList<PublishedProductResponse>>> Handle(GetAllPublishedProductsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Product> publishedProducts = await _productRepository.GetAllPublishedProductsAsync(
                query.PageNumber,
                query.PageSize,
                cancellationToken);

            PaginatedList<PublishedProductResponse> publishedProductsResponse =
                MapToPublishedProductResponsePaginatedList(publishedProducts, query);

            return Result.Success(publishedProductsResponse);
        }

        private PaginatedList<PublishedProductResponse> MapToPublishedProductResponsePaginatedList(
            PaginatedList<Product> productsPaginatedList,
            GetAllPublishedProductsWithPaginationQuery query)
        {
            List<PublishedProductResponse> publishedProductsResponse = 
                [.. productsPaginatedList.Items.Select(ProductToPublishedProductResponseMapper.Map)];

            PaginatedList<PublishedProductResponse> paginatedPublishedProductsResponse =
                PaginatedList<PublishedProductResponse>.Create(
                    publishedProductsResponse,
                    productsPaginatedList.TotalCount,
                    productsPaginatedList.PageNumber,
                    query.PageSize);

            return paginatedPublishedProductsResponse;
        }
    }
}
