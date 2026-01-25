namespace Web.Api.Endpoints.v1.Products.Product.Update
{
    public sealed record UpdateProductRequest(
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
        string MetaKeywords);
}
