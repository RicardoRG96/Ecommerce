namespace Application.Products.ProductSkus.Common.Services
{
    public sealed class DefaultSkuGenerator : ISkuGenerator
    {
        private const int CategoryMaxLength = 3;
        private const int BrandMaxLength = 4;
        private const int ProductIdPadding = 6;
        private const int VariantMaxLength = 3;
        private const int UniqueSuffixLength = 4;

        public string Generate(SkuGenerationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            ValidateContext(context);

            string categoryPart = NormalizeComponent(context.CategoryCode, CategoryMaxLength);
            string brandPart = NormalizeComponent(context.BrandCode, BrandMaxLength);
            string productPart = FormatProductId(context.ProductId);
            
            // Build parts list
            var parts = new List<string>
            {
                categoryPart,
                brandPart,
                productPart
            };

            // Add variant codes if they exist
            if (context.Variants is not null && context.Variants.Count > 0)
            {
                string variantPart = BuildVariantPart(context.Variants);
                parts.Add(variantPart);
            }

            // Always add unique suffix for uniqueness guarantee
            string uniqueSuffix = GenerateUniqueSuffix(context.GeneratedAt);
            parts.Add(uniqueSuffix);

            return string.Join("-", parts);
        }

        private static void ValidateContext(SkuGenerationContext context)
        {
            if (context.ProductId <= 0)
            {
                throw new ArgumentException("ProductId must be greater than zero.", nameof(context));
            }

            if (string.IsNullOrWhiteSpace(context.CategoryCode))
            {
                throw new ArgumentException("CategoryCode cannot be null or empty.", nameof(context));
            }

            if (string.IsNullOrWhiteSpace(context.BrandCode))
            {
                throw new ArgumentException("BrandCode cannot be null or empty.", nameof(context));
            }
        }

        private static string NormalizeComponent(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("SKU component cannot be null or empty.", nameof(value));
            }

            string normalized = new string(
                value
                    .Trim()
                    .ToUpperInvariant()
                    .Where(char.IsLetterOrDigit)
                    .Take(maxLength)
                    .ToArray()
            );

            if (normalized.Length < maxLength)
            {
                normalized = normalized.PadRight(maxLength, '0');
            }

            return normalized;
        }

        private static string FormatProductId(long productId)
        {
            return productId.ToString().PadLeft(ProductIdPadding, '0');
        }

        private static string BuildVariantPart(IReadOnlyDictionary<string, string> variants)
        {
            var variantCodes = variants
                .OrderBy(v => v.Key, StringComparer.OrdinalIgnoreCase)
                .Select(v => NormalizeComponent(v.Value, VariantMaxLength))
                .ToList();

            return string.Join("-", variantCodes);
        }

        private static string GenerateUniqueSuffix(DateTime timestamp)
        {
            // Combine timestamp with random component for guaranteed uniqueness
            long timePart = timestamp.Ticks / TimeSpan.TicksPerMillisecond;
            int randomPart = Random.Shared.Next(0, 36 * 36); // 0-1295
            
            long combined = (timePart % 46656) + randomPart; // 36^4 = 1679616
            
            return ToBase36(combined).PadLeft(UniqueSuffixLength, '0');
        }

        private static string ToBase36(long value)
        {
            const string base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = new System.Text.StringBuilder();

            do
            {
                result.Insert(0, base36Chars[(int)(value % 36)]);
                value /= 36;
            } while (value > 0);

            return result.ToString();
        }
    }
}
