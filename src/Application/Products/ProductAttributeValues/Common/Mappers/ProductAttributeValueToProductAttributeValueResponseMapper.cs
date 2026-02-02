using Domain.Entities.Products;

namespace Application.Products.ProductAttributeValues.Common.Mappers
{
    public static class ProductAttributeValueToProductAttributeValueResponseMapper
    {
        public static ProductAttributeValueResponse Map(ProductAttributeValue productAttributeValue)
        {
            return new ProductAttributeValueResponse
            {
                ProductSku = productAttributeValue.ProductSku is null
                    ? new ProductSkuResponse()
                    {
                        Id = productAttributeValue.ProductSkuId,

                        Product = productAttributeValue.ProductSku!.Product is null
                            ? new ProductResponse()
                            {
                                Id = productAttributeValue.ProductSku!.Product!.Id,
                                Name = productAttributeValue.ProductSku!.Product!.Name,
                                Slug = productAttributeValue.ProductSku!.Product!.Slug,
                                IsActive = productAttributeValue.ProductSku!.Product!.IsActive,
                                Brand = productAttributeValue.ProductSku!.Product!.Brand is null
                                    ? null
                                    : new BrandResponse()
                                    {
                                        Id = productAttributeValue.ProductSku!.Product!.Brand!.Id,
                                        Name = productAttributeValue.ProductSku!.Product!.Brand!.Name,
                                        Slug = productAttributeValue.ProductSku!.Product!.Brand!.Slug,
                                    },
                                Category = productAttributeValue.ProductSku!.Product!.Category is null
                                    ? null
                                    : new CategoryResponse()
                                    {
                                        Id = productAttributeValue.ProductSku!.Product!.Category!.Id,
                                        Name = productAttributeValue.ProductSku!.Product!.Category!.Name,
                                        Slug = productAttributeValue.ProductSku!.Product!.Category!.Slug,
                                    },
                            }
                            : null,

                        SkuCode = productAttributeValue.ProductSku!.SkuCode,
                        BarCode = productAttributeValue.ProductSku!.BarCode,
                        Price = productAttributeValue.ProductSku!.Price,
                        Weight = productAttributeValue.ProductSku!.Weight,
                        Length = productAttributeValue.ProductSku!.Length,
                        Width = productAttributeValue.ProductSku!.Width,
                        Height = productAttributeValue.ProductSku!.Height,
                        IsActive = productAttributeValue.ProductSku!.IsActive,
                        DisplayOrder = productAttributeValue.ProductSku!.DisplayOrder,
                    }
                    : null,

                AttributeCode = productAttributeValue.AttributeValue?.Attribute?.Code,
                Value = productAttributeValue.AttributeValue?.Value,
                Name = productAttributeValue.AttributeValue?.Attribute?.Name,
                DataType = productAttributeValue.AttributeValue?.Attribute?.DataType,
                IsVariant = productAttributeValue.AttributeValue?.Attribute?.IsVariant ?? false,
            };
        }
    }
}
