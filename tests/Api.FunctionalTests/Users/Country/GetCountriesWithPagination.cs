using Api.FunctionalTests.Abstractions;
using Application.Users.Countries.GetWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Country
{
    public class GetCountriesWithPagination : BaseFunctionalTest
    {
        public GetCountriesWithPagination(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan_1()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{countriesBaseUrl}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{countriesBaseUrl}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{countriesBaseUrl}?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Countries_WhenPageNumber_And_PageSize_Values_AreCorrect()
        {
            PaginatedList<CountryResponse>? countries =
                await HttpClient.GetFromJsonAsync<PaginatedList<CountryResponse>>($"{countriesBaseUrl}?pageNumber=1&pageSize=10");

            countries.Should().NotBeNull();
            countries.Items.Count.Should().Be(2);
            countries.TotalPages.Should().Be(1);
            countries.HasNextPage.Should().BeFalse();
            countries.HasPreviousPage.Should().BeFalse();
        }
    }
}
