using Application.Abstractions.Messaging;

namespace Application.Products.Category.Create
{
    public sealed record CreateCategoryCommand(
        long ParentId,
        string Name,
        string Description,
        string ImageUrl,
        string Icon,
        string DisplayOrder,
        bool IsActive,
        bool IsVisibleInMenu,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords) : ICommand<long>;
}
