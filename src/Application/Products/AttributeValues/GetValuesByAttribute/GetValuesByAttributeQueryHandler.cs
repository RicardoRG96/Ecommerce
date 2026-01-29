using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Mappers;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.GetValuesByAttribute
{
    internal sealed class GetValuesByAttributeQueryHandler : IQueryHandler<GetValuesByAttributeQuery, List<AttributeValueResponse>>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;

        public GetValuesByAttributeQueryHandler(IAttributeValueRepository attributeValueRepository)
        {
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<Result<List<AttributeValueResponse>>> Handle(GetValuesByAttributeQuery query, CancellationToken cancellationToken)
        {
            List<AttributeValue>? attributeValues = await _attributeValueRepository.GetValuesByAttributeAsync(
                query.AttributeId, 
                cancellationToken);

            List<AttributeValueResponse> attributeValueResponses = attributeValues
                .Select(AttributeValueToAttributeValueResponseMapper.Map)
                .ToList();

            return Result.Success(attributeValueResponses);
        }
    }
}
