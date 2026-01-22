using Application.Abstractions.Messaging;

namespace Application.Products.Categories.Update
{
    public sealed record UpdateCategoryCommand(
        long CategoryId,
        string Name,
        string Description,
        string ImageUrl,
        string Icon,
        int DisplayOrder,
        bool IsVisibleInMenu,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords) : ICommand;
}
