using Application.Abstractions.Data.Repositories.Products;
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

        public Task<Result> Validate(string code, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
