using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Addresses.Update;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Addresses.Update
{
    public class UpdateAddressCommandTests
    {
        private static readonly UpdateAddressCommand _command =
            new(1, 1, "UpdatedAddress", "UpdatedCity", "UpdatedStreet", "UpdatedNumber", "UpdatedApartament", "UpdatedReference", "UpdatedPostalCode");
        private readonly Address _address;
        private readonly Country _country;
        private readonly Municipality _municipality;
        private readonly UpdateAddressCommandHandler _handler;
        private readonly IAddressRepository _addressRepositoryMock;
        private readonly IMunicipalityRepository _municipalityRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateAddressCommandTests()
        {
            _addressRepositoryMock = Substitute.For<IAddressRepository>();
            _municipalityRepositoryMock = Substitute.For<IMunicipalityRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _address = new()
            {
                AddressId = _command.AddressId,
                CountryId = 1,
                MunicipalityId = _command.MunicipalityId,
                Title = "TestAddress",
                City = "TestCity"
            };
            _country = new() { CountryId = 1, Name = "TestCountry" };
            _municipality = new() { MunicipalityId = _command.MunicipalityId, RegionId = 1, Name = "TestMuni" };
            _handler = new(_addressRepositoryMock, _municipalityRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenAddressDoesNotExist()
        {
            long notExistingAddressId = 2500;
            UpdateAddressCommand invalidCommand = _command with { AddressId = notExistingAddressId };

            _addressRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.AddressId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AddressErrors.NotFound(invalidCommand.AddressId));
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenMunicipalityDoesNotExist()
        {
            long notExistingMunicipalityId = 2500;
            UpdateAddressCommand invalidCommand = _command with { MunicipalityId = notExistingMunicipalityId };

            _addressRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.MunicipalityId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(MunicipalityErrors.NotFound(invalidCommand.MunicipalityId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAddressAndMunicipalityExists()
        {
            _addressRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenAddressAndMunicipalityExists()
        {
            _addressRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            _municipalityRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.MunicipalityId), Arg.Any<CancellationToken>())
                .Returns(_municipality);

            await _handler.Handle(_command, default);

            _addressRepositoryMock
                .Received(1)
                .Update(_address);
        }
    }
}
