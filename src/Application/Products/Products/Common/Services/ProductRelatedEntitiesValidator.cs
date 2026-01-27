using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Domain.Errors.Products;
using SharedKernel;

namespace Application.Products.Products.Common.Services
{
    internal sealed class ProductRelatedEntitiesValidator : IProductRelatedEntitiesValidator
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductTaxCategoryRepository _productTaxCategoryRepository;

        public ProductRelatedEntitiesValidator(
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            IProductTaxCategoryRepository productTaxCategoryRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _productTaxCategoryRepository = productTaxCategoryRepository;
        }

        public async Task<Result> ValidateRelatedEntitiesAsync(
            long brandId,
            long categoryId,
            long productTaxCategoryId,
            CancellationToken cancellationToken)
        {
            // Execute operations sequentially to avoid DbContext threading issues
            Brand? brand = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
            if (brand is null)
                return Result.Failure(BrandErrors.NotFound(brandId));

            if (!brand.IsActive)
                return Result.Failure(ProductErrors.BrandNotActive);

            Category? category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category is null)
                return Result.Failure(CategoryErrors.NotFound(categoryId));

            if (!category.IsActive)
                return Result.Failure(ProductErrors.CategoryNotActive);

            ProductTaxCategory? productTaxCategory = await _productTaxCategoryRepository.GetByIdAsync(productTaxCategoryId, cancellationToken);
            if (productTaxCategory is null)
                return Result.Failure(ProductTaxCategoryErrors.NotFound(productTaxCategoryId));

            if (!productTaxCategory.IsActive)
                return Result.Failure(ProductErrors.ProductTaxCategoryNotActive);

            return Result.Success();
        }

        public async Task<Result> ValidateProductNameUniquenessAsync(
            string name,
            long? excludeProductId = null,
            CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByNameAsync(name, cancellationToken);

            // For updates, allow same name if it's the same product
            if (product is not null && product.Id != excludeProductId)
                return Result.Failure(ProductErrors.DuplicatedProductName);

            return Result.Success();
        }
    }
}
