using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Countries.Delete;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Countries.Delete
{
    public class DeleteCountryCommandTests
    {
        private static readonly DeleteCountryCommand _command = new(1L);
        private readonly Country _country;
        private readonly DeleteCountryCommandHandler _handler;
        private readonly ICountryRepository _countryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public DeleteCountryCommandTests()
        {
            _countryRepositoryMock = Substitute.For<ICountryRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _country = new() { CountryId = _command.Id, Name = "TestCountry" };
            _handler = new(_countryRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenCountryDoesNotExist()
        {
            long notExistingCountryId = 2500L;
            DeleteCountryCommand invalidCommand = _command with { Id = notExistingCountryId };

            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidCommand.Id), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result result = await _handler.Handle(invalidCommand, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CountryErrors.NotFound(invalidCommand.Id));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCountryExists()
        {
            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.Id), Arg.Any<CancellationToken>())
                .Returns(_country);

            Result result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenCountryExists()
        {
            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.Id), Arg.Any<CancellationToken>())
                .Returns(_country);

            await _handler.Handle(_command, default);

            _countryRepositoryMock
                .Received(1)
                .Delete(_country);
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenCountryExists()
        {
            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _command.Id), Arg.Any<CancellationToken>())
                .Returns(_country);

            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
