using Application.Abstractions.Messaging;

namespace Application.Products.Products.Create
{
    public sealed record CreateProductCommand(
        string Name,
        string Description,
        string ShortDescription,
        long BrandId,
        long CategoryId,
        long ProductTaxCategoryId,
        bool IsActive,
        bool IsFeatured,
        bool IsDigital,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords) : ICommand<long>;
}
