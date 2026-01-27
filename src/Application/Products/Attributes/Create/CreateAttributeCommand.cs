using Application.Abstractions.Messaging;

namespace Application.Products.Attributes.Create
{
    public sealed record CreateAttributeCommand(
        string Code,
        string Name,
        string Description,
        string DataType,
        bool IsVariant,
        bool IsFilterable,
        bool IsRequired,
        int DisplayOrder,
        bool IsActive) : ICommand<long>;
}
