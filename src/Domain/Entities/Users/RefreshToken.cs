namespace Domain.Entities.Users
{
    public sealed class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public long UserId { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public IDomainUser User { get; set; }
    }
}
