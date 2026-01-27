using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Products.Attributes.GetAll
{
    internal sealed class GetAllAttributesQueryHandler : IQueryHandler<GetAllAttributesQuery, List<AttributeResponse>>
    {
        private readonly IAttributeRepository _attributeRepository;

        public GetAllAttributesQueryHandler(IAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public async Task<Result<List<AttributeResponse>>> Handle(GetAllAttributesQuery query, CancellationToken cancellationToken)
        {
            List<Domain.Entities.Products.Attribute> attributes = await _attributeRepository.GetAllAsync(cancellationToken);

            List<AttributeResponse> responses = attributes.Select(attribute => new AttributeResponse
            {
                Id = attribute.Id,
                Code = attribute.Code,
                Name = attribute.Name,
                Description = attribute.Description,
                DataType = attribute.DataType,
                IsVariant = attribute.IsVariant,
                IsFilterable = attribute.IsFilterable,
                IsRequired = attribute.IsRequired,
                DisplayOrder = attribute.DisplayOrder,
                IsActive = attribute.IsActive
            }).ToList();

            return Result.Success(responses);
        }
    }
}
