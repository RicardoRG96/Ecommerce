using Application.Abstractions.Messaging;

namespace Application.Products.Categories.ChangeCategoryParent
{
    public sealed record ChangeCategoryParentCommand(
        long CategoryId,
        long NewParentId) : ICommand;
}
