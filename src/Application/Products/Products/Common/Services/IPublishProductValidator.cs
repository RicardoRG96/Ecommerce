using SharedKernel;

namespace Application.Products.Products.Common.Services
{
    public interface IPublishProductValidator
    {
        Task<Result> ValidateCanBePublishedAsync(long productId, CancellationToken cancellationToken);
    }
}
