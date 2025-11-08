using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Countries.Delete;
using Application.Users.Countries.Update;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Countries.Update
{
    public class UpdateCountryCommandTests
    {
        private static readonly UpdateCountryCommand _command = new(1L, "UpdatedTestCountry");
        private readonly Country _country;
        private readonly UpdateCountryCommandHandler _handler;
        private readonly ICountryRepository _countryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public UpdateCountryCommandTests()
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
            UpdateCountryCommand invalidCommand = _command with { Id  = notExistingCountryId };

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
            _country.Name.Should().Be(_command.Name);
        }
    }
}
