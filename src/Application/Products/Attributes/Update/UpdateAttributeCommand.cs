using Application.Abstractions.Messaging;

namespace Application.Products.Attributes.Update
{
    public sealed record UpdateAttributeCommand(
        long AttributeId,
        string Code,
        string Name,
        string Description,
        string DataType,
        bool IsVariant,
        bool IsFilterable,
        bool IsRequired,
        int DisplayOrder) : ICommand;
}
