using Application.Abstractions.Messaging;

namespace Application.Products.AttributeValues.GetById
{
    public sealed record GetAttributeValueByIdQuery(long Id) : IQuery<AttributeValueResponse>;
}
