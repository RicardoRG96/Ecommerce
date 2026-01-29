using Domain.Entities.Products;

namespace Application.Products.AttributeValues.Common.Mappers
{
    public static class AttributeValueToAttributeValueResponseMapper
    {
        public static AttributeValueResponse Map(AttributeValue attributeValue)
        {
            return new()
            {
                Id = attributeValue.Id,
                Attribute = attributeValue.Attribute is not null
                    ? new AttributeResponse
                    {
                        Id = attributeValue.Attribute.Id,
                        Code = attributeValue.Attribute.Code,
                        Name = attributeValue.Attribute.Name,
                        Description = attributeValue.Attribute.Description,
                        DataType = attributeValue.Attribute.DataType,
                        IsVariant = attributeValue.Attribute.IsVariant,
                        IsFilterable = attributeValue.Attribute.IsFilterable,
                        IsRequired = attributeValue.Attribute.IsRequired,
                        DisplayOrder = attributeValue.Attribute.DisplayOrder,
                        IsActive = attributeValue.Attribute.IsActive
                    }
                    : null,
                Value = attributeValue.Value,
                NumericValue = attributeValue.NumericValue,
                BooleanValue = attributeValue.BooleanValue,
                DisplayOrder = attributeValue.DisplayOrder,
                IsActive = attributeValue.IsActive
            };
        }
    }
}
