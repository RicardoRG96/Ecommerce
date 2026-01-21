using Application.Abstractions.Messaging;

namespace Application.Products.Brand.Create
{
    public sealed record CreateBrandCommand(
        string Name,
        string Slug,
        string Description,
        string LogoUrl,
        string BannerUrl,
        string WebsiteUrl,
        bool IsActive,
        bool IsFeatured,
        int DisplayOrder,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords) : ICommand<long>;
}
