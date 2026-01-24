using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.Products.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.Update
{
    internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductRelatedEntitiesValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository, 
            IProductRelatedEntitiesValidator validator, 
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _validator = validator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdAsync(
                command.ProductId,
                cancellationToken);

            if (product is null)
            {
                return Result.Failure(ProductErrors.NotFound(command.ProductId));
            }

            Result nameValidation = await _validator.ValidateProductNameUniquenessAsync(
                command.Name,
                command.ProductId,
                cancellationToken);

            if (nameValidation.IsFailure)
            {
                return Result.Failure(nameValidation.Error);
            }

            Result entitiesValidation = await _validator.ValidateRelatedEntitiesAsync(
                command.BrandId,
                command.CategoryId,
                command.ProductTaxCategoryId,
                cancellationToken);

            if  (entitiesValidation.IsFailure)
            {
                return Result.Failure(entitiesValidation.Error);
            }

            product.Update(
                command.Name,
                command.Description,
                command.ShortDescription,
                command.BrandId,
                command.CategoryId,
                command.ProductTaxCategoryId,
                command.IsFeatured,
                command.IsDigital,
                command.MetaTitle,
                command.MetaDescription,
                command.MetaKeywords);

            _productRepository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
