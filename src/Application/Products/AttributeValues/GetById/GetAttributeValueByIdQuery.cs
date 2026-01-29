using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Common.Mappers;

namespace Application.Products.AttributeValues.GetById
{
    public sealed record GetAttributeValueByIdQuery(long Id) : IQuery<AttributeValueResponse>;
}
