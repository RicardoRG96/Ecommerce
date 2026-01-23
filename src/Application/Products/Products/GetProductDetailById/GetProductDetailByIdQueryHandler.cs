using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.GetProductDetailById
{
    internal sealed class GetProductDetailByIdQueryHandler : IQueryHandler<GetProductDetailByIdQuery, ProductDetailResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductDetailByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDetailResponse>> Handle(GetProductDetailByIdQuery query, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetProductDetailByIdAsync(query.Id, cancellationToken);

            if (product is null)
            {
                return Result.Failure<ProductDetailResponse>(ProductErrors.NotFound(query.Id));
            }

            ProductDetailResponse response = ProductToProductDetailMapper.Map(product);

            return Result.Success(response);
        }
    }
}
