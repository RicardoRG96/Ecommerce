using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Products.ProductAttributeValues.Assign
{
    internal sealed class AssignAttributeValueToSkuCommandHandler : ICommandHandler<AssignAttributeValueToSkuCommand>
    {
        public Task<Result> Handle(AssignAttributeValueToSkuCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
