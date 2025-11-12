using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Addresses.Create;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Addresses.Create
{
    public class CreateAddressCommandTests
    {
        private static readonly CreateAddressCommand _command =
            new(1, 1, "TestAddress", "TestCity", "TestStreet", "TestNumber", "TestApartament", "TestReference", "TestPostalCode");
        private readonly Address _address;
        private readonly Country _country;
        private readonly Municipality _municipality;
        private readonly CreateAddressCommandHandler _handler;
        private readonly IAddressRepository _addressRepositoryMock;
        private readonly ICountryRepository _countryRepositoryMock;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateAddressCommandTests()
        {
            _addressRepositoryMock = Substitute.For<IAddressRepository>();
            _countryRepositoryMock = Substitute.For<ICountryRepository>();
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _address = new()
            {
                AddressId = 1,
                CountryId = _command.CountryId,
                MunicipalityId = _command.MunicipalityId,
                Title = _command.Title
            };
            _country = new() { CountryId = _command.CountryId, Name = "TestCountry" };
            _municipality = new() { MunicipalityId = _command.MunicipalityId, RegionId = 1, Name = "TestMuni" };
            _handler = new(_addressRepositoryMock, _countryRepositoryMock, _municipalityRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenCountryIdDoesNotExist()
        {
            long notExistingCountryId = 2500;
            CreateAddressCommand invalidCommand = _command with { CountryId = notExistingCountryId };

            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.CountryId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CountryErrors.NotFound(invalidCommand.CountryId));
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenMunicipalityIdDoesNotExist()
        {
            long notExistingMunicipalityId = 2500;
            CreateAddressCommand invalidCommand = _command with { MunicipalityId = notExistingMunicipalityId };

            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CountryId), Arg.Any<CancellationToken>())
                .Returns(_country);

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.MunicipalityId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(MunicipalityErrors.NotFound(invalidCommand.MunicipalityId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCountryIdAndMunicipalityIdExists()
        {
            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CountryId), Arg.Any<CancellationToken>())
                .Returns(_country);

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenCountryIdAndMunicipalityIdExists()
        {
            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.CountryId), Arg.Any<CancellationToken>())
                .Returns(_country);

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            Result<long> result = await _handler.Handle(_command, default);

            await _addressRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Address>(a => a.AddressId == result.Value), Arg.Any<CancellationToken>());
        }
    }
}
