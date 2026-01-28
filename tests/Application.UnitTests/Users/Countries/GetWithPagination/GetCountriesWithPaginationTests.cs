using Application.Abstractions.Data.Repositories.Users;
using Application.Users.Countries.GetWithPagination;
using Domain.Entities.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Users.Countries.GetWithPagination
{
    public class GetCountriesWithPaginationTests
    {
        private static readonly GetCountriesWithPaginationQuery _query = new(1, 2);
        private readonly PaginatedList<Country> _countries;
        private List<Country>? _items;
        private readonly GetCountriesWithPaginationQueryHandler _handler;
        private readonly ICountryRepository _countryRepositoryMock;

        public GetCountriesWithPaginationTests()
        {
            _countryRepositoryMock = Substitute.For<ICountryRepository>();

            CreateCountryItems();
            _countries = PaginatedList<Country>.Create(_items!, _items!.Count, _query.PageNumber, _query.PageSize);
            _handler = new(_countryRepositoryMock);
        }

        private void CreateCountryItems()
        {
            _items = new List<Country>
            {
                new() { CountryId = 1L, Name = "TestCountry1" },
                new() { CountryId = 2L, Name = "TestCountry2" },
                new() { CountryId = 3L, Name = "TestCountry3" }
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnAnEmptyList_WhenThereAreNoCountries()
        {
            PaginatedList<Country> emptyPaginatedList = PaginatedList<Country>.Create([], 0, 1, 3);

            _countryRepositoryMock
                .GetAllWithPaginationAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(emptyPaginatedList);

            Result<PaginatedList<CountryResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_ReturnAListWithElements_WhenThereAreCountries()
        {
            _countryRepositoryMock
                .GetAllWithPaginationAsync(
                    Arg.Is<int>(pn => pn == _query.PageNumber),
                    Arg.Is<int>(ps => ps == _query.PageSize),
                    Arg.Any<CancellationToken>())
                .Returns(_countries);

            Result<PaginatedList<CountryResponse>> result = await _handler.Handle(_query, default);

            result.Value.Items.Count.Should().Be(_items!.Count);
        }
    }
}
