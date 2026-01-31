using Domain.Entities.Products;

namespace Application.Products.ProductSkus.Common.Mappers
{
    public static class ProductSkuToProductSkuResponseMapper
    {
        public static ProductSkuResponse Map(ProductSku sku)
        {
            return new ProductSkuResponse()
            {
                Id = sku.Id,

                Product = sku.Product is not null
                    ? new ProductResponse
                    {
                        Id = sku.Product.Id,
                        Name = sku.Product.Name,
                        Slug = sku.Product.Slug,
                        Description = sku.Product.Description,
                        ShortDescription = sku.Product.ShortDescription,
                        IsActive = sku.Product.IsActive,
                        MetaTitle = sku.Product.MetaTitle,
                        MetaDescription = sku.Product.MetaDescription,

                        Brand = sku.Product.Brand is not null
                            ? new BrandResponse
                            {
                                Id = sku.Product.Brand.Id,
                                Name = sku.Product.Brand.Name,
                                Slug = sku.Product.Brand.Slug,
                                LogoUrl = sku.Product.Brand.LogoUrl,
                            }
                            : null,

                        Category = sku.Product.Category is not null
                            ? new CategoryResponse
                            {
                                Id = sku.Product.Category.Id,
                                Name = sku.Product.Category.Name,
                                Slug = sku.Product.Category.Slug,
                            }
                            : null,
                    }
                    : null,

                SkuCode = sku.SkuCode,
                BarCode = sku.BarCode,
                Price = sku.Price,
                Weight = sku.Weight,
                Length = sku.Length,
                Width = sku.Width,
                Height = sku.Height,
                IsActive = sku.IsActive,
                DisplayOrder = sku.DisplayOrder,

                ProductAttributeValues = sku.ProductAttributeValues?
                        .Select(attrValue => new ProductAttributeValueResponse
                        {
                            Value = attrValue.AttributeValue.Value,
                            Name = attrValue.AttributeValue.Attribute.Name,
                            IsVariant = attrValue.AttributeValue.Attribute.IsVariant,
                        })
                        .ToList(),

                ProductGalleries = sku.ProductGalleries?
                        .Select(gallery => new ProductGalleryResponse
                        {
                            MediaUrl = gallery.MediaUrl,
                            IsPrimary = gallery.IsPrimary,
                            DisplayOrder = gallery.DisplayOrder,
                            AltText = gallery.AltText
                        })
                        .ToList(),

                ProductSkuDiscounts = sku.DiscountSkus?
                        .Select(discountSku => new ProductSkuDiscountsResponse
                        {
                            Id = discountSku.DiscountId,
                            Code = discountSku.Discount.Code,
                            Name = discountSku.Discount.Name,
                            DiscountType = discountSku.Discount.DiscountType,
                            Value = discountSku.Discount.Value,
                            IsActive = discountSku.Discount.IsActive,
                        })
                        .ToList(),

                ProductSkuStocks = sku.ProductSkuStocks ?
                        .Select(stock => new ProductSkuStockResponse
                        {
                            Id = stock.Id,
                            WarehouseId = stock.WarehouseId,
                            Stock = stock.Stock,
                            ReservedStock = stock.ReservedStock,
                            MinStock = stock.MinStock,
                            IsActive = stock.IsActive,
                        })
                        .ToList()
            };
        }
    }
}
