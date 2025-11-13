using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Data.UnitOfWork;
using Application.Users.Countries.Create;
using Domain.Entities.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Countries.Create
{
    public class CreateCountryCommandTests
    {
        private static readonly CreateCountryCommand _command = new("TestCountry");
        private readonly CreateCountryCommandHandler _handler;
        private readonly ICountryRepository _countryRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        public CreateCountryCommandTests()
        {
            _countryRepositoryMock = Substitute.For<ICountryRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handler = new(_countryRepositoryMock, _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCountryNameIsPresent()
        {
            Result<long> result = await _handler.Handle(_command, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_CallRepository_WhenCountryNameIsPresent()
        {
            Result<long> result = await _handler.Handle(_command, default);

            await _countryRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Country>(c => c.CountryId == result.Value), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenCountryNameIsPresent()
        {
            await _handler.Handle(_command, default);

            await _unitOfWorkMock
                .Received(1)
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
