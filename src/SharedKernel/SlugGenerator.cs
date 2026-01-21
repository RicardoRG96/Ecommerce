using System.Globalization;
using System.Text;

namespace SharedKernel
{
    public static class SlugGenerator
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var normalized = input.ToLowerInvariant()
                                  .Normalize(NormalizationForm.FormD);

            var buffer = new char[normalized.Length];
            var length = 0;
            var previousWasDash = false;

            foreach (var c in normalized)
            {
                if (Char.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                    continue;

                char current;

                if (char.IsLetterOrDigit(c))
                {
                    current = c;
                    previousWasDash = false;
                }
                else
                {
                    if (previousWasDash)
                        continue;

                    current = '-';
                    previousWasDash = true;
                }

                buffer[length++] = current;
            }

            var result = new string(buffer, 0, length).Trim('-');

            return result;
        }
    }
}
