namespace Web.Api.Endpoints.v1.Products.Category.Create
{
    public sealed record CreateCategoryRequest(
        long ParentId,
        string Name,
        string? Description,
        string? ImageUrl,
        string? Icon,
        int DisplayOrder,
        bool IsActive,
        bool IsVisibleInMenu,
        string? MetaTitle,
        string? MetaDescription,
        string? MetaKeywords
    );
}
