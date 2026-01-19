using SharedKernel;

namespace Domain.Entities.Products
{
    public sealed class ProductSkuStock : BaseAuditableEntity
    {
        public long Id { get; set; }
        public long ProductSkuId { get; set; }
        public long WarehouseId { get; set; }
        public int Stock { get; set; }
        public int ReservedStock { get; set; }
        public int MinStock { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        public ProductSku ProductSku { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
    }
}
