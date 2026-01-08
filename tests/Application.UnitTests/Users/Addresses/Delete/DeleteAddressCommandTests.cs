using Application.Abstractions.Authentication;
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
        private readonly IUserContext _userContextMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeleteAddressCommandTests()
        {
            _addressRepositoryMock = Substitute.For<IAddressRepository>();
            _userContextMock = Substitute.For<IUserContext>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _address = new()
            {
                AddressId = _command.AddressId,
                CountryId = 1,
                MunicipalityId = 1,
                Title = "TestAddress",
                City = "TestCity",
                AddressUsers = 
                [
                    new()
                    {
                        AddressId = _command.AddressId,
                        Address = _address,
                        ApplicationUserId = 1,
                        IsDefault = false
                    }
                ]
            };
            _handler = new(_addressRepositoryMock, _userContextMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenAddressDoesNotExist()
        {
            long notExistingAddressId = 2500;
            DeleteAddressCommand invalidCommand = _command with { AddressId = notExistingAddressId };

            _addressRepositoryMock
                .GetByIdIncludingAddressUserAsync(Arg.Is<long>(id => id == invalidCommand.AddressId), Arg.Any<CancellationToken>())
                .ReturnsNull();

            _userContextMock
                .UserId
                .Returns(1);

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AddressErrors.NotFound(invalidCommand.AddressId));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenAddressExists()
        {
            _addressRepositoryMock
                .GetByIdIncludingAddressUserAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            _userContextMock
                .UserId
                .Returns(1);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenAddressExists()
        {
            _addressRepositoryMock
                .GetByIdIncludingAddressUserAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            _userContextMock
                .UserId
                .Returns(1);

            await _handler.Handle(_command, default);

            _addressRepositoryMock
                .Received(1)
                .Delete(_address);
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenAddressExists()
        {
            _addressRepositoryMock
                .GetByIdIncludingAddressUserAsync(Arg.Is<long>(id => id == _command.AddressId), Arg.Any<CancellationToken>())
                .Returns(_address);

            _userContextMock
                .UserId
                .Returns(1);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
