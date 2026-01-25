using Application.Abstractions.Messaging;

namespace Application.Products.Products.Update
{
    public sealed record UpdateProductCommand(
        long ProductId,
        string Name,
        string Description,
        string ShortDescription,
        long BrandId,
        long CategoryId,
        long ProductTaxCategoryId,
        bool IsFeatured,
        bool IsDigital,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords) : ICommand;
}
