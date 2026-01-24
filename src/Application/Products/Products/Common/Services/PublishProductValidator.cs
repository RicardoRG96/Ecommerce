using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using SharedKernel;

namespace Application.Products.Products.Common.Services
{
    internal sealed class PublishProductValidator : IPublishProductValidator
    {
        private readonly IProductRepository _productRepository;

        public PublishProductValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> ValidateCanBePublishedAsync(long productId, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdIncludingRelatedEntitiesAsync(productId, cancellationToken);


        }
    }
}
