using Application.Abstractions.Messaging;

namespace Application.Products.Attributes.GetById
{
    public sealed record GetAttributeByIdQuery(long AttributeId) : IQuery<AttributeResponse>;
}
