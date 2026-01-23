namespace Application.Products.Categories.GetCategoryTree
{
    public sealed class CategoryTreeResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? ImageUrl { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public List<CategoryTreeResponse> Children { get; set; } = [];
    }
}
