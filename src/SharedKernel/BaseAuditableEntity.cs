namespace SharedKernel
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string? CreatedBy { get; set; } // utilizarlo en un interceptor (Infrastructure/Persistence/Interceptors)
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
