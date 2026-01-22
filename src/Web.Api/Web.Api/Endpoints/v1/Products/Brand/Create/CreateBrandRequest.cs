namespace Web.Api.Endpoints.v1.Products.Brand.Create
{
    public record CreateBrandRequest(
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
