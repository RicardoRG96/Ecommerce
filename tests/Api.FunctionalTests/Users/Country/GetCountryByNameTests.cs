using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Countries.GetByName;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Country
{
    public class GetCountryByNameTests : BaseFunctionalTest
    {
        public GetCountryByNameTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCountryNameDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}/name/noCountry");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Country_WhenCountryNameExists()
        {
            SetAdminAuthentication();

            CountryResponse? country = await HttpClient.GetFromJsonAsync<CountryResponse>($"{ApiRoutes.Locations.Countries}/name/chile");

            country.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}/name/chile");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Countries}/name/chile");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

    }
}
