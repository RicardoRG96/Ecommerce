using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Countries.GetByName;
using Domain.Entities.Users;
using Domain.Errors.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users.Countries.GetByName
{
    public class GetCountryByNameQueryTests
    {
        private static readonly GetCountryByNameQuery _query = new("TestCountry");
        private readonly Country _country;
        private readonly GetCountryByNameQueryHandler _handler;
        private readonly ICountryRepository _countryRepositoryMock;

        public GetCountryByNameQueryTests()
        {
            _countryRepositoryMock = Substitute.For<ICountryRepository>();

            _country = new() { CountryId = 1L, Name = _query.Name };
            _handler = new(_countryRepositoryMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenCountryDoesNotExist()
        {
            string notExistingCountryName = "invalidQuery";
            GetCountryByNameQuery invalidQuery = _query with { Name = notExistingCountryName };

            _countryRepositoryMock
                .GetByNameAsync(Arg.Is<string>(name => name == invalidQuery.Name), Arg.Any<CancellationToken>())
                .ReturnsNull();

            Result<CountryResponse> result = await _handler.Handle(invalidQuery, default);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CountryErrors.NotFoundByName);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenCountryExists()
        {
            _countryRepositoryMock
                .GetByNameAsync(Arg.Is<string>(name => name == _query.Name), Arg.Any<CancellationToken>())
                .Returns(_country);

            Result<CountryResponse> result = await _handler.Handle(_query, default);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Name.Should().Be(_query.Name);
        }
    }
}
