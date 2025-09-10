namespace Infrastructure.Models.Users
{
    public sealed class CountryModel
    {
        public long CountryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<AddressModel> Addresses { get; } = new List<AddressModel>();
    }
}
