using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Municipalities.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Municipality
{
    public class GetMunicipalityByIdTests : BaseFunctionalTest
    {
        public GetMunicipalityByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{municipalitiesBaseUrl}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Municipality_WhenMunicipalityIdExists()
        {
            MunicipalityResponse? municipality = 
                await HttpClient.GetFromJsonAsync<MunicipalityResponse>($"{municipalitiesBaseUrl}/1");

            municipality.Should().NotBeNull();
        }
    }
}
