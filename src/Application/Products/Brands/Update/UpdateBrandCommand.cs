using Application.Abstractions.Messaging;

namespace Application.Products.Brands.Update
{
    public sealed record UpdateBrandCommand(
        long BrandId,
        string Name,
        string Description,
        string LogoUrl,
        string BannerUrl,
        string WebsiteUrl,
        bool IsActive,
        bool IsFeatured,
        int DisplayOrder,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords) : ICommand;
}
