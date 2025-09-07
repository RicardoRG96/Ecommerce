namespace Infrastructure.Models.Users
{
    public sealed class CountryModel
    {
        public long CountryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<AddressModel> Addresses { get; } = new List<AddressModel>();
    }
}
