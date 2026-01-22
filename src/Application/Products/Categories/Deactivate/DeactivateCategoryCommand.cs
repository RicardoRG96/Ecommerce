using Application.Abstractions.Messaging;

namespace Application.Products.Categories.Deactivate
{
    public sealed record DeactivateCategoryCommand(long CategoryId) : ICommand;
}
