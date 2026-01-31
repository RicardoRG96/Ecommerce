using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Mappers;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.ProductSkus.GetById
{
    internal sealed class GetProductSkuByIdQueryHandler : IQueryHandler<GetProductSkuByIdQuery, ProductSkuResponse>
    {
        private readonly IProductSkuRepository _productSkuRepository;

        public GetProductSkuByIdQueryHandler(IProductSkuRepository productSkuRepository)
        {
            _productSkuRepository = productSkuRepository;
        }

        public async Task<Result<ProductSkuResponse>> Handle(GetProductSkuByIdQuery query, CancellationToken cancellationToken)
        {
            ProductSku? productSku = await _productSkuRepository.GetByIdWithRelatedEntitiesAsync(query.Id, cancellationToken);

            if (productSku is null)
            {
                return Result.Failure<ProductSkuResponse>(ProductSkuErrors.NotFound(query.Id));
            }

            ProductSkuResponse response = ProductSkuToProductSkuResponseMapper.Map(productSku);

            return Result.Success(response);
        }
    }
}
