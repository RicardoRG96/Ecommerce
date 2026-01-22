using Application.Abstractions.Messaging;

namespace Application.Products.Categories.Activate
{
    public sealed record ActivateCategoryCommand(long CategoryId) : ICommand;
}
