namespace Web.Api.Endpoints.v1.Products.Brand.Update
{
    public record UpdateBrandRequest(
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
        string MetaKeywords);
}
