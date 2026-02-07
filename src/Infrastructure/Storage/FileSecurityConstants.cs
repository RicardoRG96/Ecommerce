using System.Text.RegularExpressions;

namespace Infrastructure.Storage
{
    internal static class FileSecurityConstants
    {
        public const int MaxFileNameLength = 255;

        public static readonly Regex FileNameValidationRegex = new(
            @"^[a-zA-Z0-9._\-\s]+$",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(100));

        public static readonly Regex FileNameSanitizationRegex = new(
            @"[^a-zA-Z0-9_\-]",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(100));

        // Windows reserved file names that cannot be used.
        // Security policy - based on Windows OS restrictions.
        public static readonly HashSet<string> WindowsReservedNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

        public static readonly char[] PathTraversalCharacters = { '/', '\\' };

        public const string PathTraversalPattern = "..";

        public static bool ContainsPathTraversal(string fileName)
        {
            return fileName.Contains(PathTraversalPattern) ||
                   fileName.IndexOfAny(PathTraversalCharacters) >= 0;
        }

        public static bool IsWindowsReservedName(string fileNameWithoutExtension)
        {
            return WindowsReservedNames.Contains(fileNameWithoutExtension);
        }
    }
}