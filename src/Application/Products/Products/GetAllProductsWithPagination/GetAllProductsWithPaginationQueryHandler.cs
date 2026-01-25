using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Products.GetAllProductsWithPagination
{
    internal sealed class GetAllProductsWithPaginationQueryHandler
        : IQueryHandler<GetAllProductsWithPaginationQuery, PaginatedList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsWithPaginationQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<PaginatedList<ProductResponse>>> Handle(GetAllProductsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Product> products = await _productRepository.GetAllAsync(
                query.PageNumber, query.PageSize, cancellationToken);

            PaginatedList<ProductResponse> productsResponse = MapToProductResponsePaginatedList(products, query);

            return Result.Success(productsResponse);
        }

        private PaginatedList<ProductResponse> MapToProductResponsePaginatedList(
            PaginatedList<Product> productsPaginatedList,
            GetAllProductsWithPaginationQuery query)
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
