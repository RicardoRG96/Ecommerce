using Application.Abstractions.Messaging;

namespace Application.Products.Categories.GetCategoryTree
{
    public sealed record GetCategoryTreeQuery : IQuery<List<CategoryTreeResponse>>;
}
