using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Addresses;
using Application.Users.Addresses.GetByTitle;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Addresses.GetByTitle
{
    public class GetAddressByTitleQueryTests
    {
        private static readonly GetAddressByTitleQuery _query = new("TestAddress");
        private Address? _address;
        private readonly GetAddressByTitleQueryHandler _handler;
        private readonly IAddressRepository _addressRepositoryMock;
        private readonly IUserContext _userContextMock;

        public GetAddressByTitleQueryTests()
        {
            _addressRepositoryMock = Substitute.For<IAddressRepository>();
            _userContextMock = Substitute.For<IUserContext>();

            CreateAddressWithReferences();
            _handler = new(_addressRepositoryMock, _userContextMock);
        }

        private void CreateAddressWithReferences()
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
            _address = new()
            {
                AddressId = 1,
                CountryId = country.CountryId,
                MunicipalityId = municipality.MunicipalityId,
                Country = country,
                Municipality = municipality,
                Title = _query.Title,
                AddressUsers =
                [
                    new()
                    {
                        AddressId = 1,
                        Address = _address,
                        ApplicationUserId = 1,
                        IsDefault = false
                    }
                ]
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenAddressDoesNotExist()
        {
            string notExistingAddressTitle = "invalidTitle";
            GetAddressByTitleQuery invalidQuery = _query with { Title  = notExistingAddressTitle };

            _addressRepositoryMock
                .GetByTitleAsync(Arg.Is<string>(t => t == invalidQuery.Title), Arg.Any<CancellationToken>())
                .ReturnsNull();

            _userContextMock
                .UserId
                .Returns(1);

            Result result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AddressErrors.NotFoundByTitle);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAddressExists()
        {
            _addressRepositoryMock
                .GetByTitleAsync(Arg.Is<string>(t => t == _query.Title), Arg.Any<CancellationToken>())
                .Returns(_address);

            _userContextMock
                .UserId
                .Returns(1);

            Result<AddressResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Title.Should().Be(_query.Title);
        }
    }
}
