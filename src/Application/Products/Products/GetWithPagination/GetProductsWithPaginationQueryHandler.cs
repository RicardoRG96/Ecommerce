using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Products.GetWithPagination
{
    internal sealed class GetProductsWithPaginationQueryHandler
        : IQueryHandler<GetProductsWithPaginationQuery, PaginatedList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsWithPaginationQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<PaginatedList<ProductResponse>>> Handle(GetProductsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Product> products = await _productRepository.GetAllAsync(
                query.PageNumber, query.PageSize, cancellationToken);

            PaginatedList<ProductResponse> productsResponse = MapToProductResponsePaginatedList(products, query);

            return Result.Success(productsResponse);
        }

        private PaginatedList<ProductResponse> MapToProductResponsePaginatedList(
            PaginatedList<Product> productsPaginatedList,
            GetProductsWithPaginationQuery query)
        {
            List<ProductResponse> productsResponse = [.. productsPaginatedList.Items.Select(ProductToProductResponseMapper.Map)];

            PaginatedList<ProductResponse> paginatedProductsResponse =
                PaginatedList<ProductResponse>.Create(
                    productsResponse,
                    productsPaginatedList.TotalCount,
                    productsPaginatedList.PageNumber,
                    query.PageSize);

            return paginatedProductsResponse;
        }
    }
}
