using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.GetProductDetail
{
    internal sealed class GetProductDetailQueryHandler : IQueryHandler<GetProductDetailQuery, ProductDetailResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductDetailQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDetailResponse>> Handle(GetProductDetailQuery query, CancellationToken cancellationToken)
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
