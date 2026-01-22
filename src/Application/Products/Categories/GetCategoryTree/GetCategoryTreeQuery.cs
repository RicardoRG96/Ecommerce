using Application.Abstractions.Messaging;

namespace Application.Products.Categories.GetCategoryTree
{
    public sealed record GetCategoryTreeQuery(long CategoryId) : IQuery<CategoryTreeResponse>;
}
