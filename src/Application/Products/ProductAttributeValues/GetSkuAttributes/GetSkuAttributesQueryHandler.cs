using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Application.Products.ProductAttributeValues.Common.Mappers;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.ProductAttributeValues.GetSkuAttributes
{
    internal sealed class GetSkuAttributesQueryHandler 
        : IQueryHandler<GetSkuAttributesQuery, List<ProductAttributeValueResponse>>
    {
        private readonly IProductAttributeValueRepository _productAttributeValueRepository;

        public GetSkuAttributesQueryHandler(IProductAttributeValueRepository productAttributeValueRepository)
        {
            _productAttributeValueRepository = productAttributeValueRepository;
        }

        public async Task<Result<List<ProductAttributeValueResponse>>> Handle(
            GetSkuAttributesQuery query, 
            CancellationToken cancellationToken)
        {
            List<ProductAttributeValue>? productAttributeValues = await _productAttributeValueRepository.GetSkuAttributesAsync(
                query.ProductSkuId, 
                cancellationToken);

            List<ProductAttributeValueResponse> response = productAttributeValues.
                Select(ProductAttributeValueToProductAttributeValueResponseMapper.Map)
                .ToList();

            return response;
        }
    }
}
