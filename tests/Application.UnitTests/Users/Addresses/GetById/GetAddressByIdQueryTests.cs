using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Addresses;
using Application.Users.Addresses.GetById;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Addresses.GetById
{
    public class GetAddressByIdQueryTests
    {
        private static readonly GetAddressByIdQuery _query = new(1);
        private Address? _address;
        private readonly GetAddressByIdQueryHandler _handler;
        private readonly IAddressRepository _addressRepositoryMock;
        private readonly IUserContext _userContextMock;

        public GetAddressByIdQueryTests()
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
                AddressId = _query.AddressId,
                CountryId = country.CountryId,
                MunicipalityId = municipality.MunicipalityId,
                Country = country,
                Municipality = municipality,
                Title = "TestAddress",
                AddressUsers =
                [
                    new()
                    {
                        AddressId = _query.AddressId,
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
            long notExistingAddressId = 2500;
            GetAddressByIdQuery invalidQuery = _query with { AddressId = notExistingAddressId };

            _addressRepositoryMock
                .GetByIdIncludingAddressUserAsync(Arg.Is<long>(id => id == invalidQuery.AddressId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            _userContextMock
                .UserId
                .Returns(1);

            Result result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AddressErrors.NotFound(invalidQuery.AddressId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAddressExists()
        {
            _addressRepositoryMock
                .GetByIdIncludingAddressUserAsync(Arg.Is<long>(id => id == _query.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            _userContextMock
                .UserId
                .Returns(1);

            Result<AddressResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Title.Should().Be(_address.Title);
        }
    }
}
