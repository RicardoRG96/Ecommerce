using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Addresses;
using Application.Users.Addresses.GetWithPagination;
using Domain.Entities.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Addresses.GetWithPagination
{
    public class GetAddressesWithPaginationQueryTests
    {
        private static readonly GetAddressesWithPaginationQuery _query = new(1, 2);
        private readonly PaginatedList<Address> _addresses;
        private List<Address>? _items;
        private readonly GetAddressesWithPaginationQueryHandler _handler;
        private readonly IAddressRepository _addressRepositoryMock;

        public GetAddressesWithPaginationQueryTests()
        {
            _addressRepositoryMock = Substitute.For<IAddressRepository>();

            CreateAddressesItems();
            _addresses = PaginatedList<Address>.Create(_items!, _items!.Count, _query.PageNumber, _query.PageSize);
            _handler = new(_addressRepositoryMock);
        }

        private void CreateAddressesItems()
        {
            Country country = new() { CountryId = 1, Name = "TestCountry" };
            Region region = new() { RegionId = 1, Name = "TestRegion" };
            Municipality municipality = new()
            {
                MunicipalityId = 1,
                Name = "TestMunicipality",
                RegionId = region.RegionId,
                Region = region
            };

            _items = new List<Address>
            {
                new()
                {
                    AddressId = 1,
                    CountryId = country.CountryId,
                    MunicipalityId = municipality.MunicipalityId,
                    Country = country,
                    Municipality = municipality,
                    Title = "TestAddress1"
                },
                new()
                {
                    AddressId = 2,
                    CountryId = country.CountryId,
                    MunicipalityId = municipality.MunicipalityId,
                    Country = country,
                    Municipality = municipality,
                    Title = "TestAddress2"
                },
                new()
                {
                    AddressId = 3,
                    CountryId = country.CountryId,
                    MunicipalityId = municipality.MunicipalityId,
                    Country = country,
                    Municipality = municipality,
                    Title = "TestAddress3"
                }
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnAnEmptyList_WhenThereAreNoAddresses()
        {
            PaginatedList<Address> emptyPaginatedList = PaginatedList<Address>.Create([], 0, 1, 3);

            _addressRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(emptyPaginatedList);

            Result<PaginatedList<AddressResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_ReturnAListWithElements_WhenThereAreAddresses()
        {
            _addressRepositoryMock
                .GetAllAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(_addresses);

            Result<PaginatedList<AddressResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Count.Should().Be(_items!.Count);
        }
    }
}
