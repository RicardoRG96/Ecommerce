using Domain.Entities.Products;

namespace Application.Abstractions.Data.Repositories.Products
{
    public interface IProductGalleryRepository : IRepository<ProductGallery>
    {
        Task<ProductGallery?> GetPrimaryMedia(long productGalleryId, CancellationToken cancellationToken);
         Task<List<ProductGallery>> GetByProductIdAsync(long productId, CancellationToken cancellationToken);
    }
}
