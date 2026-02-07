using SharedKernel;

namespace Application.Abstractions.Storage
{
    public interface IFileStorageService
    {
        Task<Result<string>> UploadAsync(
            Stream stream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(
            string fileName,
            CancellationToken cancellationToken = default);

        string GetFileUrl(string fileName);

        Task<Result<bool>> FileExistsAsync(
            string fileName,
            CancellationToken cancellationToken = default);

        Task<Result<Dictionary<string, string>>> UploadMultipleAsync(
            IEnumerable<(Stream Stream, string FileName, string ContentType)> files,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteMultipleAsync(
            IEnumerable<string> fileNames,
            CancellationToken cancellationToken = default);
    }
}
