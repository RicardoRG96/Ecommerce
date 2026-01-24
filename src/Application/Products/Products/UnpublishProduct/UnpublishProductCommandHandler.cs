using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.UnpublishProduct
{
    internal sealed class UnpublishProductCommandHandler : ICommandHandler<UnpublishProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnpublishProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UnpublishProductCommand command, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

            if (product is null)
            {
                return Result.Failure(ProductErrors.NotFound(command.ProductId));
            }

            if (!product.IsPublished)
            {
                return Result.Success();
            }

            // TODO: Add validation to check if the product have active orders, etc.

            product.Unpublish();

            _productRepository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
