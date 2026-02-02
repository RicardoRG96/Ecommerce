namespace Application.Abstractions.Search
{
    public sealed class ProductSearchModel
    {
        public required string ObjectID { get; init; }
        public required string Name { get; init; }
        public required string  Slug { get; init; }
        public required string Description { get; init; }
        public string? SkuCode { get; init; }
        public string? Brand { get; init; }
        public string? Category { get; init; }
        public decimal Price { get; init; }
        public string? ImageUrl { get; init; }
        public Dictionary<string, object>? Attributes { get; init; }
    }
}
