namespace Infrastructure.Storage
{
    public sealed class BlobStorageOptions
    {
        public const string SectionName = "AzureBlobStorage";
        public string ConnectionString { get; init; } = string.Empty;
        public string ContainerName { get; init; } = string.Empty;
        public long MaxFileSizeInBytes { get; init; } = 10 * 1024 * 1024;
        public List<string> AllowedContentTypes { get; init; } = new()
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/gif",
            "video/mp4",
            "video/webm"
        };

        public List<string> AllowedExtensions { get; init; } = new()
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".webp",
            ".mp4",
            ".webm",
            ".mov",
            ".avi"
        };

        public Dictionary<string, List<string>> ExtensionContentTypeMappings { get; init; } = new()
        {
            { ".jpg", new List<string> { "image/jpeg", "image/jpg" } },
            { ".jpeg", new List<string> { "image/jpeg", "image/jpg" } },
            { ".png", new List<string> { "image/png" } },
            { ".gif", new List<string> { "image/gif" } },
            { ".webp", new List<string> { "image/webp" } },
            { ".mp4", new List<string> { "video/mp4" } },
            { ".webm", new List<string> { "video/webm" } },
            { ".mov", new List<string> { "video/quicktime" } },
            { ".avi", new List<string> { "video/x-msvideo", "video/avi" } }
        };
    }
}