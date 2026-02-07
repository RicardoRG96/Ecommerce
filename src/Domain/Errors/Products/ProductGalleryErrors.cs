using SharedKernel;

namespace Domain.Errors.Products
{
    public static class ProductGalleryErrors
    {
        public static Error NotFound(long productGalleryId) => Error.NotFound(
            "ProductGallery.NotFound",
            $"The ProductGallery with the id = '{productGalleryId}' was not found");

        public static readonly Error ProductNotFound = Error.NotFound(
            "ProductGallery.ProductNotFound",
            "The provided productId was not found");

        public static readonly Error SkuNotFound = Error.NotFound(
            "ProductGallery.SkuNotFound",
            "The provided skuId was not found");

        public static readonly Error SkuNotBelongsToProduct = Error.NotFound(
            "ProductGallery.SkuNotBelongsToProduct",
            "The provided skuId does not belong to the product");
    }
}
