using Application.Abstractions.Data.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Persistence.Database;

namespace Infrastructure.Persistence.Repositories.Products
{
    public class ProductGalleryRepository : Repository<ProductGallery>, IProductGalleryRepository
    {
        public ProductGalleryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
