using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.Create
{
    internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, long>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductTaxCategoryRepository _productTaxCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository, 
            IBrandRepository brandRepository, 
            ICategoryRepository categoryRepository, 
            IProductTaxCategoryRepository productTaxCategoryRepository, 
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _productTaxCategoryRepository = productTaxCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByNameAsync(command.Name, cancellationToken);

            if (product is not null)
            {
                return Result.Failure<long>(ProductErrors.DuplicatedProductName);
            }

            Brand? brand = await _brandRepository.GetByIdAsync(command.BrandId, cancellationToken);

            if (brand is null)
            {
                return Result.Failure<long>(BrandErrors.NotFound(command.BrandId));
            }

            if (!brand.IsActive)
            {
                return Result.Failure<long>(ProductErrors.BrandNotActive);
            }

            Category? category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result.Failure<long>(CategoryErrors.NotFound(command.CategoryId));
            }

            if (!category.IsActive)
            {
                return Result.Failure<long>(ProductErrors.CategoryNotActive);
            }

            ProductTaxCategory? productTaxCategory = 
                await _productTaxCategoryRepository.GetByIdAsync(command.ProductTaxCategoryId, cancellationToken);

            if (productTaxCategory is null)
            {
                return Result.Failure<long>(ProductTaxCategoryErrors.NotFound(command.ProductTaxCategoryId));
            }

            if (!productTaxCategory.IsActive)
            {
                return Result.Failure<long>(ProductErrors.ProductTaxCategoryNotActive);
            }

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
