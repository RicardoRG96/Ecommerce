using Application.Abstractions.Authentication;
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
        private readonly PaginatedList<AddressUser> _addresses;
        private List<AddressUser>? _items;
        private readonly GetAddressesWithPaginationQueryHandler _handler;
        private readonly IAddressUserRepository _addressUserRepositoryMock;
        private readonly IUserContext _userContextMock;

        public GetAddressesWithPaginationQueryTests()
        {
            _addressUserRepositoryMock = Substitute.For<IAddressUserRepository>();
            _userContextMock = Substitute.For<IUserContext>();

            CreateAddressesItems();
            _addresses = PaginatedList<AddressUser>.Create(_items!, _items!.Count, _query.PageNumber, _query.PageSize);
            _handler = new(_addressUserRepositoryMock, _userContextMock);
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

            Address address1 = new()
            {
                AddressId = 1,
                CountryId = country.CountryId,
                MunicipalityId = municipality.MunicipalityId,
                Country = country,
                Municipality = municipality,
                Title = "TestAddress1"
            };

            Address address2 = new()
            {
                AddressId = 2,
                CountryId = country.CountryId,
                MunicipalityId = municipality.MunicipalityId,
                Country = country,
                Municipality = municipality,
                Title = "TestAddress2"
            };

            Address address3 = new()
            {
                AddressId = 3,
                CountryId = country.CountryId,
                MunicipalityId = municipality.MunicipalityId,
                Country = country,
                Municipality = municipality,
                Title = "TestAddress3"
            };

            _items = new List<AddressUser>
            {
                new()
                {
                    Address = address1,
                    AddressId = address1.AddressId,
                    ApplicationUserId = 1,
                    IsDefault = false
                },
                new()
                {
                    Address = address2,
                    AddressId = address2.AddressId,
                    ApplicationUserId = 1,
                    IsDefault = false
                },
                new()
                {
                    Address = address3,
                    AddressId = address3.AddressId,
                    ApplicationUserId = 1,
                    IsDefault = false
                }
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnAnEmptyList_WhenThereAreNoAddresses()
        {
            PaginatedList<AddressUser> emptyPaginatedList = PaginatedList<AddressUser>.Create([], 0, 1, 3);

             _addressUserRepositoryMock
                .GetByUserIdAsync(
                    Arg.Is<long>(id => id == 1),
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
            _addressUserRepositoryMock
                .GetByUserIdAsync(
                Arg.Is<long>(id => id == 1),
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(_addresses);

            Result<PaginatedList<AddressResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Count.Should().Be(_items!.Count);
        }
    }
}
