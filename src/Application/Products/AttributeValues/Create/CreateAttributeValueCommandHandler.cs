using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Products.AttributeValues.Create
{
    internal sealed class CreateAttributeValueCommandHandler : ICommandHandler<CreateAttributeValueCommand, long>
    {
        private readonly IAttributeValueRepository _attributeValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Task<Result<long>> Handle(CreateAttributeValueCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
