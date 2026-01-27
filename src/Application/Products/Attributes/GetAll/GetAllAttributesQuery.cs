using Application.Abstractions.Messaging;

namespace Application.Products.Attributes.GetAll
{
    public sealed record GetAllAttributesQuery() : IQuery<List<AttributeResponse>>;
}
