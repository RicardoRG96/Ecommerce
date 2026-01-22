using Application.Abstractions.Messaging;

namespace Application.Products.Categories.Create
{
    public sealed record CreateCategoryCommand(
        long ParentId,
        string Name,
        string Description,
        string ImageUrl,
        string Icon,
        int DisplayOrder,
        bool IsActive,
        bool IsVisibleInMenu,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords) : ICommand<long>;
}
