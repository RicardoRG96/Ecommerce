using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Countries.GetById;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Countries.GetById
{
    public class GetCountryByIdQueryTests
    {
        private static readonly GetCountryByIdQuery _query = new(1L);
        private readonly Country _country;
        private readonly GetCountryByIdQueryHandler _handler;
        private readonly ICountryRepository _countryRepositoryMock;

        public GetCountryByIdQueryTests()
        {
            _countryRepositoryMock = Substitute.For<ICountryRepository>();

            _country = new() { CountryId = _query.Id, Name = "TestCountry" };
            _handler = new(_countryRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenCountryDoesNotExist()
        {
            long notExistingCountryId = 2500L;
            GetCountryByIdQuery invalidQuery = _query with { Id = notExistingCountryId };

            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == invalidQuery.Id), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result<CountryResponse> result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CountryErrors.NotFound(invalidQuery.Id));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCountryExists()
        {
            _countryRepositoryMock
                .GetByIdAsync(Arg.Is<long>(id => id == _query.Id), Arg.Any<CancellationToken>())
                .Returns(_country);

            Result<CountryResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Name.Should().Be(_country.Name);
        }
    }
}
