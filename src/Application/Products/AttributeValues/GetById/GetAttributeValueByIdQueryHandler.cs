using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Mappers;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.AttributeValues.GetById
{
    internal sealed class GetAttributeValueByIdQueryHandler : IQueryHandler<GetAttributeValueByIdQuery, AttributeValueResponse>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;

        public GetAttributeValueByIdQueryHandler(IAttributeValueRepository attributeValueRepository)
        {
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<Result<AttributeValueResponse>> Handle(GetAttributeValueByIdQuery query, CancellationToken cancellationToken)
        {
            AttributeValue? attributeValue = await _attributeValueRepository.GetByIdWithRelatedEntitiesAsync(
                query.Id, 
                cancellationToken);

            if (attributeValue is null)
            {
                return Result.Failure<AttributeValueResponse>(AttributeValueErrors.NotFound(query.Id));
            }

            AttributeValueResponse response = AttributeValueToAttributeValueResponseMapper.Map(attributeValue);

            return Result.Success(response);
        }
    }
}
