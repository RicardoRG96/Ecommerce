using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Attributes.Deactivate
{
    internal sealed class DeactivateAttributeCommandHandler : ICommandHandler<DeactivateAttributeCommand>
    {
        private readonly IAttributeRepository _attributeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateAttributeCommandHandler(
            IAttributeRepository attributeRepository, 
            IUnitOfWork unitOfWork)
        {
            _attributeRepository = attributeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeactivateAttributeCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.Products.Attribute? attribute = await _attributeRepository.GetByIdAsync(
                command.AttributeId, 
                cancellationToken);

            if (attribute is null)
            {
                return Result.Failure(AttributeErrors.NotFound(command.AttributeId));
            }

            if (attribute.IsActive is false)
            {
                return Result.Success();
            }

            attribute.Deactivate();

            _attributeRepository.Update(attribute);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
