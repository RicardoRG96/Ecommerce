namespace Web.Api.Endpoints.v1.Products.Product.Create
{
    public sealed record CreateProductRequest(
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
        string MetaKeywords);
}
