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
    }
}