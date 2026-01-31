using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Mappers;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.ProductSkus.GetSkusByProduct
{
    internal sealed class GetSkusByProductQueryHandler : IQueryHandler<GetSkusByProductQuery, List<ProductSkuResponse>>
    {
        private readonly IProductSkuRepository _productSkuRepository;

        public GetSkusByProductQueryHandler(IProductSkuRepository productSkuRepository)
        {
            _productSkuRepository = productSkuRepository;
        }

        public async Task<Result<List<ProductSkuResponse>>> Handle(GetSkusByProductQuery query, CancellationToken cancellationToken)
        {
            List<ProductSku> productSkus = await _productSkuRepository.GetSkusByProductIdAsync(
                query.ProductId, 
                cancellationToken);

            List<ProductSkuResponse> productSkuResponses = productSkus.Select(
                ProductSkuToProductSkuResponseMapper.Map).ToList();

            return Result.Success(productSkuResponses);
        }
    }
}
