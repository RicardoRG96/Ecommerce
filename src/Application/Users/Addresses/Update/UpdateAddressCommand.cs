using Application.Abstractions.Messaging;

namespace Application.Users.Addresses.Update
{
    public sealed class UpdateAddressCommand : ICommand
    {
        public long MunicipalityId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? Apartament { get; set; }
        public string? Reference { get; set; }
        public string PostalCode { get; set; }
    }
}
