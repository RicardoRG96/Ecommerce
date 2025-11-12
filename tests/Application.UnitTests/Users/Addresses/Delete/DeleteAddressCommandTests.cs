using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Addresses.Delete;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Addresses.Delete
{
    public class DeleteAddressCommandTests
    {
        private static readonly DeleteAddressCommand _command = new(1);
        private readonly Address _address;
        private readonly DeleteAddressCommandHandler _handler;
        private readonly IAddressRepository _addressRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeleteAddressCommandTests()
        {
            _addressRepositoryMock = Substitute.For<IAddressRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _address = new()
            {
                AddressId = _command.AddressId,
                CountryId = 1,
                MunicipalityId = 1,
                Title = "TestAddress",
                City = "TestCity"
            };
            _handler = new(_addressRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenAddressDoesNotExist()
        {
            long notExistingAddressId = 2500;
            DeleteAddressCommand invalidCommand = _command with { AddressId = notExistingAddressId };

            _addressRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.AddressId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AddressErrors.NotFound(invalidCommand.AddressId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAddressExists()
        {
            _addressRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenAddressExists()
        {
            _addressRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            await _handler.Handle(_command, default);

            _addressRepositoryMock
                .Received(1)
                .Delete(_address);
        }
    }
}
