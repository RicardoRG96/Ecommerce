using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Countries.GetWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Headers;
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
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Countries_WhenPageNumber_And_PageSize_Values_AreCorrect()
        {
            SetAdminAuthentication();

            PaginatedList<CountryResponse>? countries =
                await HttpClient.GetFromJsonAsync<PaginatedList<CountryResponse>>($"{ApiRoutes.Locations.Countries}?pageNumber=1&pageSize=10");

            countries.Should().NotBeNull();
            countries.Items.Count.Should().Be(2);
            countries.TotalPages.Should().Be(1);
            countries.HasNextPage.Should().BeFalse();
            countries.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
