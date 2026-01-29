namespace Application.Products.ProductSkus.Common.Services
{
    public sealed class DefaultSkuGenerator : ISkuGenerator
    {
        private const int ProductSkuCodeLength = 6;

        public string Generate(SkuGenerationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            string category = Normalize(context.CategoryCode, 3);
            string brand = Normalize(context.BrandCode, 4);
            string productSkuCode = context.ProductSkuId
                .ToString()
                .PadLeft(ProductSkuCodeLength, '0');

            string? variantPart = BuildVariantPart(context.Variants);

            return string.Join(
                "-",
                new[] { category, brand, productSkuCode, variantPart }
                    .Where(p => !string.IsNullOrWhiteSpace(p))
            );
        }

        private static string Normalize(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("SKU components cannot be empty");

            return new string(
                value
                    .Trim()
                    .ToUpperInvariant()
                    .Where(char.IsLetterOrDigit)
                    .Take(maxLength)
                    .ToArray()
            );
        }

        private static string? BuildVariantPart(
            IReadOnlyDictionary<string, string>? variants)
        {
            if (variants is null || variants.Count == 0)
                return null;

            return string.Join(
                "-",
                variants
                    .OrderBy(v => v.Key)
                    .Select(v => Normalize(v.Value, 3))
            );
        }
    }
}
