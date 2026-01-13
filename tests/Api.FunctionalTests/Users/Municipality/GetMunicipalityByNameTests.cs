using Api.FunctionalTests.Abstractions;
using Application.Users.Municipalities.GetByName;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Municipality
{
    public class GetMunicipalityByNameTests : BaseFunctionalTest
    {
        public GetMunicipalityByNameTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityNameDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{municipalitiesBaseUrl}/name/noMuni");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Municipality_WhenMunicipalityNameExists()
        {
            SetAdminAuthentication();

            MunicipalityResponse? municipality =
                await HttpClient.GetFromJsonAsync<MunicipalityResponse>($"{municipalitiesBaseUrl}/name/Santiago");

            municipality.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{municipalitiesBaseUrl}/name/Santiago");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{municipalitiesBaseUrl}/name/Santiago");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
