using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.Products.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.PublishProduct
{
    internal sealed class PublishProductCommandHandler : ICommandHandler<PublishProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPublishProductValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public PublishProductCommandHandler(
            IProductRepository productRepository, 
            IPublishProductValidator validator,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PublishProductCommand command, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

            if (product is null)
            {
                return Result.Failure(ProductErrors.NotFound(command.ProductId));
            }

            Result publishValidation = await _validator.ValidateCanBePublishedAsync(command.ProductId, cancellationToken);

            if (publishValidation.IsFailure)
            {
                return Result.Failure(publishValidation.Error);
            }

            product.IsActive = true;

            _productRepository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
