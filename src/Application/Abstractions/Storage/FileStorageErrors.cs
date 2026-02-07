using SharedKernel;

namespace Application.Abstractions.Storage
{
    public static class FileStorageErrors
    {
        public static Error UploadFailed(string reason) => Error.Problem(
            "FileStorage.UploadFailed",
            $"Failed to upload file to cloud storage: {reason}");

        public static Error DeleteFailed(string reason) => Error.Problem(
            "FileStorage.DeleteFailed",
            $"Failed to delete file from cloud storage: {reason}");

        public static Error FileNotFound(string fileName) => Error.NotFound(
            "FileStorage.FileNotFound",
            $"File '{fileName}' was not found in cloud storage");

        public static Error InvalidContentType(string contentType, string[] allowedTypes) => Error.Failure(
            "FileStorage.InvalidContentType",
            $"Content type '{contentType}' is not allowed. Allowed types: {string.Join(", ", allowedTypes)}");

        public static Error FileSizeExceeded(long fileSize, long maxSize) => Error.Failure(
            "FileStorage.FileSizeExceeded",
            $"File size {fileSize} bytes exceeds maximum allowed size of {maxSize} bytes");

        public static Error BatchUploadFailed(int successCount, int failedCount) => Error.Problem(
            "FileStorage.BatchUploadFailed",
            $"Batch upload partially failed. Succeeded: {successCount}, Failed: {failedCount}");

        public static Error StorageNotAccessible(string provider) => Error.Problem(
            "FileStorage.StorageNotAccessible",
            $"Unable to access {provider} storage. Check connection and permissions");

        public static Error EmptyStream => Error.Failure(
            "FileStorage.EmptyStream",
            "Cannot upload an empty stream");

        public static Error InvalidFileName(string fileName) => Error.Failure(
            "FileStorage.InvalidFileName",
            $"File name '{fileName}' is invalid");
    }
}