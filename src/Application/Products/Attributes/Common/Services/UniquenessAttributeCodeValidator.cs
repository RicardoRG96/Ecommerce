using Application.Abstractions.Data.Repositories.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Attributes.Common.Services
{
    internal sealed class UniquenessAttributeCodeValidator : IUniquenessAttributeCodeValidator
    {
        private readonly IAttributeRepository _attributeRepository;

        public UniquenessAttributeCodeValidator(IAttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public async Task<Result> ValidateAsync(
            string code,
            long? excludeAttributeId = null,
            CancellationToken cancellationToken = default)
        {
            Domain.Entities.Products.Attribute? attribute = await _attributeRepository.GetByCodeAsync(code, cancellationToken);

            if (attribute is not null && attribute.Id != excludeAttributeId)
            {
                return Result.Failure(AttributeErrors.DuplicatedAttributeCode);
            }

            return Result.Success();
        }
    }
}
