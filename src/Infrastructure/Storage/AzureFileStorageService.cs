using Application.Abstractions.Storage;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel;

namespace Infrastructure.Storage
{
    public sealed class AzureFileStorageService : IFileStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly BlobStorageOptions _options;
        private readonly ILogger<AzureFileStorageService> _logger;

        public AzureFileStorageService(
            BlobContainerClient containerClient,
            IOptions<BlobStorageOptions> options,
            ILogger<AzureFileStorageService> logger)
        {
            _containerClient = containerClient ?? throw new ArgumentNullException(nameof(containerClient));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<string>> UploadAsync(
            Stream stream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            if (stream is null || stream.Length == 0)
            {
                _logger.LogWarning("Attempted to upload empty stream for file {FileName}", fileName);
                return Result.Failure<string>(FileStorageErrors.EmptyStream);
            }

            Result fileNameValidation = ValidateFileName(fileName);

            if (fileNameValidation.IsFailure)
            {
                _logger.LogWarning(
                    "Invalid file name {FileName}: {Error}",
                    fileName,
                    fileNameValidation.Error.Description);

                return Result.Failure<string>(fileNameValidation.Error);
            }

            Result contentTypeValidation = ValidateContentType(contentType);

            if (contentTypeValidation.IsFailure)
            {
                _logger.LogWarning(
                    "Invalid content type {ContentType} for file {FileName}",
                    contentType,
                    fileName);
                return Result.Failure<string>(contentTypeValidation.Error);
            }

            Result extensionValidation = ValidateExtensionMatchesContentType(fileName, contentType);

            if (extensionValidation.IsFailure)
            {
                _logger.LogWarning(
                    "Extension mismatch for file {FileName} with content type {ContentType}",
                    fileName,
                    contentType);

                return Result.Failure<string>(extensionValidation.Error);
            }

            Result fileSizeValidation = ValidateFileSize(stream.Length);

            if (fileSizeValidation.IsFailure)
            {
                _logger.LogWarning(
                    "File size {FileSize} exceeds maximum for file {FileName}",
                    stream.Length,
                    fileName);

                return Result.Failure<string>(fileSizeValidation.Error);
            }

            try
            {
                Result containerResult = await EnsureContainerExistsAsync(cancellationToken);
                if (containerResult.IsFailure)
                {
                    return Result.Failure<string>(containerResult.Error);
                }

                string uniqueFileName = GenerateUniqueFileName(fileName);
                BlobClient blobClient = _containerClient.GetBlobClient(uniqueFileName);

                BlobHttpHeaders blobHttpHeaders = new()
                {
                    ContentType = contentType,
                    CacheControl = "public, max-age=31536000"
                };

                BlobUploadOptions uploadOptions = new()
                {
                    HttpHeaders = blobHttpHeaders,
                    Metadata = new Dictionary<string, string>
                    {
                        { "UploadedAt", DateTime.UtcNow.ToString("o") },
                        { "OriginalFileName", fileName }
                    }
                };

                stream.Position = 0;
                await blobClient.UploadAsync(stream, uploadOptions, cancellationToken);

                _logger.LogInformation(
                    "File {FileName} uploaded successfully to Azure Blob Storage as {UniqueFileName}",
                    fileName,
                    uniqueFileName);

                return Result.Success(blobClient.Uri.ToString());
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(
                    ex,
                    "Azure RequestFailedException while uploading file {FileName}",
                    fileName);

                return Result.Failure<string>(FileStorageErrors.UploadFailed(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while uploading file {FileName}",
                    fileName);

                return Result.Failure<string>(FileStorageErrors.UploadFailed(ex.Message));
            }
        }

        public async Task<Result> DeleteAsync(string fileName, CancellationToken cancellationToken = default)
        {
            Result fileNameValidation = ValidateFileName(fileName);

            if (fileNameValidation.IsFailure)
            {
                _logger.LogWarning("Invalid file name for deletion {FileName}", fileName);
                return Result.Failure(fileNameValidation.Error);
            }

            try
            {
                BlobClient blobClient = _containerClient.GetBlobClient(fileName);

                Response<bool> response = await blobClient.DeleteIfExistsAsync(
                    cancellationToken: cancellationToken);

                if (!response.Value)
                {
                    _logger.LogWarning("File {FileName} not found for deletion", fileName);

                    return Result.Failure(FileStorageErrors.FileNotFound(fileName));
                }

                _logger.LogInformation("File {FileName} deleted successfully from Azure Blob Storage", fileName);

                return Result.Success();
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(
                    ex,
                    "Azure RequestFailedException while deleting file {FileName}",
                    fileName);

                return Result.Failure(FileStorageErrors.DeleteFailed(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while deleting file {FileName}",
                    fileName);

                return Result.Failure(FileStorageErrors.DeleteFailed(ex.Message));
            }
        }

        public string GetFileUrl(string fileName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(fileName);

            return blobClient.Uri.ToString();
        }

        public async Task<Result<bool>> FileExistsAsync(
            string fileName,
            CancellationToken cancellationToken = default)
        {
            Result fileNameValidation = ValidateFileName(fileName);

            if (fileNameValidation.IsFailure)
            {
                return Result.Failure<bool>(fileNameValidation.Error);
            }

            try
            {
                BlobClient blobClient = _containerClient.GetBlobClient(fileName);

                bool exists = await blobClient.ExistsAsync(cancellationToken);

                return Result.Success(exists);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(
                    ex,
                    "Error checking if file {FileName} exists in Azure Blob Storage",
                    fileName);

                return Result.Failure<bool>(FileStorageErrors.UploadFailed(ex.Message));
            }
        }

        public async Task<Result<Dictionary<string, string>>> UploadMultipleAsync(
            IEnumerable<(Stream Stream, string FileName, string ContentType)> files,
            CancellationToken cancellationToken = default)
        {
            var uploadedFiles = new Dictionary<string, string>();
            var filesList = files.ToList();

            if (filesList.Count == 0)
            {
                return Result.Success(uploadedFiles);
            }

            foreach (var file in filesList)
            {
                var uploadResult = await UploadAsync(
                    file.Stream,
                    file.FileName,
                    file.ContentType,
                    cancellationToken);

                if (uploadResult.IsFailure)
                {
                    _logger.LogError(
                        "Failed to upload file {FileName} in batch: {Error}",
                        file.FileName,
                        uploadResult.Error.Description);

                    await RollbackUploadsAsync(uploadedFiles.Keys, cancellationToken);

                    return Result.Failure<Dictionary<string, string>>(
                        FileStorageErrors.BatchUploadFailed(uploadedFiles.Count, filesList.Count - uploadedFiles.Count));
                }

                uploadedFiles[file.FileName] = uploadResult.Value;
            }

            _logger.LogInformation(
                "Batch upload completed successfully. {Count} files uploaded to Azure Blob Storage",
                uploadedFiles.Count);

            return Result.Success(uploadedFiles);
        }

        public async Task<Result> DeleteMultipleAsync(
            IEnumerable<string> fileNames,
            CancellationToken cancellationToken = default)
        {
            var fileNamesList = fileNames.ToList();

            if (fileNamesList.Count == 0)
            {
                return Result.Success();
            }

            var failedDeletes = new List<string>();

            foreach (var fileName in fileNamesList)
            {
                var deleteResult = await DeleteAsync(fileName, cancellationToken);

                if (deleteResult.IsFailure)
                {
                    failedDeletes.Add(fileName);

                    _logger.LogWarning(
                        "Failed to delete file {FileName}: {Error}",
                        fileName,
                        deleteResult.Error.Description);
                }
            }

            if (failedDeletes.Count != 0)
            {
                _logger.LogWarning(
                    "Batch delete completed with {FailedCount} failures out of {TotalCount}",
                    failedDeletes.Count,
                    fileNamesList.Count);

                return Result.Failure(
                    FileStorageErrors.BatchUploadFailed(
                        fileNamesList.Count - failedDeletes.Count,
                        failedDeletes.Count));
            }

            _logger.LogInformation(
                "Batch delete completed successfully. {Count} files deleted from Azure Blob Storage",
                fileNamesList.Count);

            return Result.Success();
        }

        private async Task<Result> EnsureContainerExistsAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _containerClient.CreateIfNotExistsAsync(
                    PublicAccessType.Blob,
                    cancellationToken: cancellationToken);

                return Result.Success();
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create or access container {ContainerName}",
                    _containerClient.Name);

                return Result.Failure(
                    FileStorageErrors.StorageNotAccessible(_containerClient.Name));
            }
        }

        private Result ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result.Failure(FileStorageErrors.InvalidFileName("File name is empty"));
            }

            fileName = fileName.Trim();

            if (FileSecurityConstants.ContainsPathTraversal(fileName))
            {
                _logger.LogWarning(
                    "Potential path traversal attack detected in file name: {FileName}",
                    fileName);

                return Result.Failure(
                    FileStorageErrors.InvalidFileName("File name contains invalid path characters"));
            }

            if (fileName.Length > FileSecurityConstants.MaxFileNameLength)
            {
                return Result.Failure(
                    FileStorageErrors.InvalidFileName(
                        $"File name exceeds maximum length of {FileSecurityConstants.MaxFileNameLength} characters"));
            }

            if (!FileSecurityConstants.FileNameValidationRegex.IsMatch(fileName))
            {
                return Result.Failure(
                    FileStorageErrors.InvalidFileName(
                        "File name contains invalid characters. Only letters, numbers, dots, dashes, underscores, and spaces are allowed"));
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            if (FileSecurityConstants.IsWindowsReservedName(fileNameWithoutExtension))
            {
                return Result.Failure(
                    FileStorageErrors.InvalidFileName(
                        $"'{fileNameWithoutExtension}' is a reserved system name"));
            }

            string extension = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(extension))
            {
                return Result.Failure(
                    FileStorageErrors.InvalidFileName("File must have an extension"));
            }

            if (!_options.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return Result.Failure(
                    FileStorageErrors.InvalidFileName(
                        $"File extension '{extension}' is not allowed. Allowed: {string.Join(", ", _options.AllowedExtensions)}"));
            }

            return Result.Success();
        }

        private Result ValidateContentType(string contentType)
        {
            if (!_options.AllowedContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
            {
                return Result.Failure(
                    FileStorageErrors.InvalidContentType(
                        contentType,
                        _options.AllowedContentTypes.ToArray()));
            }

            return Result.Success();
        }

        private Result ValidateExtensionMatchesContentType(string fileName, string contentType)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (_options.ExtensionContentTypeMappings.TryGetValue(extension, out var validContentTypes))
            {
                if (!validContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "Content type mismatch: file {FileName} has extension {Extension} but content type {ContentType}",
                        fileName,
                        extension,
                        contentType);

                    return Result.Failure(
                        FileStorageErrors.InvalidContentType(
                            contentType,
                            validContentTypes.ToArray()));
                }
            }

            return Result.Success();
        }

        private Result ValidateFileSize(long fileSize)
        {
            if (fileSize > _options.MaxFileSizeInBytes)
            {
                return Result.Failure(
                    FileStorageErrors.FileSizeExceeded(fileSize, _options.MaxFileSizeInBytes));
            }

            return Result.Success();
        }

        private static string GenerateUniqueFileName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);

            fileNameWithoutExtension = FileSecurityConstants.FileNameSanitizationRegex.Replace(
                fileNameWithoutExtension,
                "_");

            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string guid = Guid.NewGuid().ToString("N")[..8];

            return $"{fileNameWithoutExtension}_{timestamp}_{guid}{extension}".ToLowerInvariant();
        }

        private async Task RollbackUploadsAsync(
            IEnumerable<string> fileNames,
            CancellationToken cancellationToken)
        {
            try
            {
                await DeleteMultipleAsync(fileNames, cancellationToken);
                _logger.LogWarning("Rolled back {Count} uploaded files", fileNames.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to rollback uploaded files");
            }
        }
    }
}
