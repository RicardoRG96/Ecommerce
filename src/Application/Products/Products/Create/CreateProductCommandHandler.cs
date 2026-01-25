using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.Products.Common.Services;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Products.Create
{
    internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, long>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductRelatedEntitiesValidator _validator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository, 
            IProductRelatedEntitiesValidator productRelatedEntitiesValidator,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _validator = productRelatedEntitiesValidator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Result nameValidation = await _validator.ValidateProductNameUniquenessAsync(
                command.Name, 
                cancellationToken: cancellationToken);

            if (nameValidation.IsFailure)
            {
                return Result.Failure<long>(nameValidation.Error);
            }

            Result entitiesValidation = await _validator.ValidateRelatedEntitiesAsync(
                command.BrandId,
                command.CategoryId,
                command.ProductTaxCategoryId,
                cancellationToken);

            if (entitiesValidation.IsFailure)
            {
                return Result.Failure<long>(entitiesValidation.Error);
            }

            Product newProduct = Product.Create(
                command.Name,
                command.Description,
                command.ShortDescription,
                command.BrandId,
                command.CategoryId,
                command.ProductTaxCategoryId,
                command.IsActive,
                command.IsFeatured,
                command.IsDigital,
                command.MetaTitle,
                command.MetaDescription,
                command.MetaKeywords
            );

            await _productRepository.AddAsync(newProduct, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(newProduct.Id);
        }
    }
}
