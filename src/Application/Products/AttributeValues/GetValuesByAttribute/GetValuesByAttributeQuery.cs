using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Mappers;

namespace Application.Products.AttributeValues.GetValuesByAttribute
{
    public sealed record GetValuesByAttributeQuery(long AttributeId) 
        : IQuery<List<AttributeValueResponse>>;
}
