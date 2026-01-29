using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;

namespace Application.Products.ProductSkus.Common.Services
{
    public sealed class SkuGenerationContextBuilder
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IAttributeValueRepository _attributeValueRepository;

        public SkuGenerationContextBuilder(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBrandRepository brandRepository,
            IAttributeValueRepository attributeValueRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<SkuGenerationContext> BuildAsync(
            long productSkuId,
            long productId,
            IEnumerable<long>? attributeValueIds = null,
            CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product is null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found.");
            }

            Category? category = await _categoryRepository.GetByIdAsync(product.CategoryId, cancellationToken);
            if (category is null)
            {
                throw new InvalidOperationException($"Category with ID {product.CategoryId} not found.");
            }

            Brand? brand = await _brandRepository.GetByIdAsync(product.BrandId, cancellationToken);
            if (brand is null)
            {
                throw new InvalidOperationException($"Brand with ID {product.BrandId} not found.");
            }

            var variants = await BuildVariantsDictionaryAsync(attributeValueIds, cancellationToken);

            return new SkuGenerationContext
            {
                ProductSkuId = productSkuId,
                ProductId = productId,
                CategoryCode = ExtractCode(category.Name),
                BrandCode = ExtractCode(brand.Name),
                ProductCode = product.Slug,
                Variants = variants,
                GeneratedAt = DateTime.UtcNow
            };
        }

        private async Task<IReadOnlyDictionary<string, string>?> BuildVariantsDictionaryAsync(
            IEnumerable<long>? attributeValueIds,
            CancellationToken cancellationToken)
        {
            if (attributeValueIds is null || !attributeValueIds.Any())
            {
                return null;
            }

            // Note: You'll need to add this method to IAttributeValueRepository
            var attributeValues = await _attributeValueRepository
                .GetByIdsAsync(attributeValueIds, cancellationToken);

            // For now, return null or implement according to your repository
            // This is a placeholder that you should implement
            return null;
        }

        private static string ExtractCode(string name)
        {
            // Extract meaningful code from name
            // Example: "Electronics & Gadgets" -> "ELC"
            // Example: "Samsung" -> "SAMS"

            if (string.IsNullOrWhiteSpace(name))
            {
                return "UNK";
            }

            // Try to extract first letter of each word
            var words = name.Split(new[] { ' ', '-', '_', '&' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length >= 3)
            {
                // Use first letter of first three words
                return new string(words.Take(3).Select(w => char.ToUpperInvariant(w[0])).ToArray());
            }
            else if (words.Length == 1)
            {
                // Use first letters of single word
                return new string(name.Take(4).Select(char.ToUpperInvariant).ToArray());
            }
            else
            {
                // Use combination
                return new string(words.SelectMany(w => w.Take(2)).Take(4).Select(char.ToUpperInvariant).ToArray());
            }
        }
    }
}