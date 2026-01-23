using Domain.Entities.Products;

namespace Application.Products.Products.GetProductDetail
{
    public static class ProductToProductDetailMapper
    {
        public static ProductDetailResponse Map(Product product)
        {
            return new ProductDetailResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                IsActive = product.IsActive,
                MetaTitle = product.MetaTitle,
                MetaDescription = product.MetaDescription,

                Brand = product.Brand is not null
                    ? new BrandResponse
                    {
                        Id = product.Brand.Id,
                        Name = product.Brand.Name,
                        Slug = product.Brand.Slug,
                        LogoUrl = product.Brand.LogoUrl
                    }
                    : null,

                Category = product.Category is not null
                    ? new CategoryResponse
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name,
                        Slug = product.Category.Slug
                    }
                    : null,

                ProductSkus = product.ProductSkus
                    .Select(sku => new ProductSkuResponse
                    {
                        Id = sku.Id,
                        SkuCode = sku.SkuCode,
                        Price = sku.Price,
                        ComparedAtPrice = sku.ComparedAtPrice,
                        IsActive = sku.IsActive,
                        DisplayOrder = sku.DisplayOrder,
                        ProductAttributeValues = sku.ProductAttributeValues
                            .Select(attrValue => new ProductAttributeValueResponse
                            {
                                Value = attrValue.AttributeValue.Value,
                                Name = attrValue.AttributeValue.Attribute.Name,
                                IsVariant = attrValue.AttributeValue.Attribute.IsVariant
                            })
                            .ToList(),
                        ProductGalleriesResponse = sku.ProductGalleries
                            .Select(gallery => new ProductGalleryResponse
                            {
                                MediaUrl = gallery.MediaUrl,
                                IsPrimary = gallery.IsPrimary,
                                DisplayOrder = gallery.DisplayOrder,
                                AltText = gallery.AltText
                            })
                            .ToList()
                    })
                    .ToList(),

                ProductGalleries = product.ProductGalleries
                    .Select(gallery => new ProductGalleryResponse
                    {
                        MediaUrl = gallery.MediaUrl,
                        IsPrimary = gallery.IsPrimary,
                        DisplayOrder = gallery.DisplayOrder,
                        AltText = gallery.AltText
                    })
                    .ToList()
            };
        }
    }
}
