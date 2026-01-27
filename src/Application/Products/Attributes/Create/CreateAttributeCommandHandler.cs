using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Products.Attributes.Create
{
    internal sealed class CreateAttributeCommandHandler : ICommandHandler<CreateAttributeCommand, long>
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAttributeCommandHandler(IAttributeRepository attributeRepository, IUnitOfWork unitOfWork)
        {
            _attributeRepository = attributeRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Result<long>> Handle(CreateAttributeCommand command, CancellationToken cancellationToken)
        {
            
        }
    }
}
