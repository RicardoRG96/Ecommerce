using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Messaging;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Attributes.GetById
{
    internal sealed class GetAttributeByIdQueryHandler : IQueryHandler<GetAttributeByIdQuery, AttributeResponse>
    {
        private readonly IAttributeRepository _attributeRepository;

        public GetAttributeByIdQueryHandler(IAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public async Task<Result<AttributeResponse>> Handle(GetAttributeByIdQuery query, CancellationToken cancellationToken)
        {
            Domain.Entities.Products.Attribute? attribute = await _attributeRepository.GetByIdAsync(query.AttributeId, cancellationToken);

            if (attribute is null)
            {
                return Result.Failure<AttributeResponse>(AttributeErrors.NotFound(query.AttributeId));
            }

            AttributeResponse response = new()
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
            };

            return Result.Success(response);
        }
    }
}
