using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Products.Products.Common.Services;
using Domain.Entities.Products;
using Domain.Errors.Products;
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
            Result nameValidation = await 

            Product newProduct = new Product
            {
                Name = command.Name,
                Slug = SlugGenerator.GenerateSlug(command.Name),
                Description = command.Description,
                ShortDescription = command.ShortDescription,
                BrandId = command.BrandId,
                CategoryId = command.CategoryId,
                ProductTaxCategoryId = command.ProductTaxCategoryId,
                IsActive = command.IsActive,
                IsFeatured = command.IsFeatured,
                IsDigital = command.IsDigital,
                MetaTitle = command.MetaTitle,
                MetaDescription = command.MetaDescription,
                MetaKeywords = command.MetaKeywords
            };

            await _productRepository.AddAsync(newProduct, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(newProduct.Id);
        }
    }
}
