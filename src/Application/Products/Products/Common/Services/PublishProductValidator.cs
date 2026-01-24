using SharedKernel;

namespace Application.Products.Products.Common.Services
{
    internal sealed class PublishProductValidator : IPublishProductValidator
    {


        public Task<Result> ValidateCanBePublishedAsync(long productId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
