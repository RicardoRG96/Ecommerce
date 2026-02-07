namespace Application.Products.ProductGalleries.AddMedia
{
    public static class MediaTypeMapper
    {
        public static string Map(string contentType)
        {
            if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return "Image";
            }
            else if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            {
                return "Video";
            }
            else
            {
                return "Unknown";
            }
        }
    }
}
