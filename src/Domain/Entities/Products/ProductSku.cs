using Domain.Errors.Products;
using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class ProductSku : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string? SkuCode { get; set; }
        public string? BarCode { get; set; }
        public decimal Price { get; set; }
        public decimal? ComparedAtPrice { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public Product Product { get; set; } = null!;
        public ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();
        public ICollection<DiscountSku> DiscountSkus { get; set; } = new List<DiscountSku>();
        public ICollection<ProductGallery> ProductGalleries { get; set; } = new List<ProductGallery>();
        public ICollection<ProductSkuStock> ProductSkuStocks { get; set; } = new List<ProductSkuStock>();

        public static ProductSku Create(
            long productId,
            string? skuCode,
            string? barCode,
            decimal price,
            decimal? cost,
            decimal? weight,
            decimal? length,
            decimal? width,
            decimal? height,
            bool isActive,
            int displayOrder)
        {
            return new ProductSku
            {
                ProductId = productId,
                SkuCode = skuCode,
                BarCode = barCode,
                Price = price,
                ComparedAtPrice = null,
                Cost = cost,
                Weight = weight,
                Length = length,
                Width = width,
                Height = height,
                IsActive = isActive,
                DisplayOrder = displayOrder
            };
        }

        public void Update(
            long productId,
            string? barCode,
            decimal price,
            decimal? cost,
            decimal? weight,
            decimal? length,
            decimal? width,
            decimal? height,
            int displayOrder)
        {
            ProductId = productId;
            BarCode = barCode;
            Price = price;
            Cost = cost;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;
            DisplayOrder = displayOrder;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public Result ReorderGallery(IReadOnlyList<long> orderedIds)
        {
            if (orderedIds.Count != ProductGalleries.Count)
            {
                return Result.Failure(ProductSkuErrors.InvalidOrderGallery);
            }

            if (orderedIds.Distinct().Count() != orderedIds.Count)
            {
                return Result.Failure(ProductSkuErrors.DuplicateIdsInGallery);
            }

            Dictionary<long, ProductGallery> galleryMap = ProductGalleries.ToDictionary(g => g.Id);

            if (orderedIds.Any(id => !galleryMap.ContainsKey(id)))
            {
                return Result.Failure(ProductSkuErrors.ForeignItemInGallery);
            }

            for (int i = 0; i < orderedIds.Count; i++)
            {
                galleryMap[orderedIds[i]].DisplayOrder = i + 1;
            }

            return Result.Success();
        }
    }
}
