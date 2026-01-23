namespace Web.Api.Endpoints.v1.Products.Category.Update
{
    public sealed record UpdateCategoryRequest(
        string Name,
        string Description,
        string ImageUrl,
        string Icon,
        int DisplayOrder,
        bool IsVisibleInMenu,
        string MetaTitle,
        string MetaDescription,
        string MetaKeywords);
}
