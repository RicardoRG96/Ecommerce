using System.Globalization;
using System.Text;

namespace SharedKernel
{
    public static class SlugGenerator
    {
        public static string GenerateSlug(string name)
        {
            return name
                .ToLowerInvariant()
                .Normalize(NormalizationForm.FormD)
                .Where(c => Char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Select(c => char.IsLetterOrDigit(c) ? c : '-')
                .Aggregate(new StringBuilder(), (sb, c) =>
                    sb.Length == 0 || sb[^1] != '-' ? sb.Append(c) : sb)
                .ToString()
                .Trim('-');
        }
    }
}
