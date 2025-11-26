using Api.FunctionalTests.Abstractions;
using Application.Users.Municipalities.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Municipality.Update;

namespace Api.FunctionalTests.Users.Municipality
{
    public class UpdateMunicipalityTests : BaseFunctionalTest
    {
        private static readonly UpdateMunicipalityRequest _request = new(1, "UpdatedName");

        public UpdateMunicipalityTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionIdIsMissing()
        {
            UpdateMunicipalityRequest invalidRequest = _request with { RegionId = 0 };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameIsMissing()
        {
            UpdateMunicipalityRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameExceedsTheMaximumLength()
        {
            UpdateMunicipalityRequest invalidRequest = 
                _request with { Name = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/2500", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            UpdateMunicipalityRequest invalidRequest = _request with { RegionId = 2500 };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_MunicipalityIdExists()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateName_WhenRequestIsValid_And_MunicipalityIdExists()
        {
            await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/1", _request);

            MunicipalityResponse? municipality = 
                await HttpClient.GetFromJsonAsync<MunicipalityResponse>($"{municipalitiesBaseUrl}/1");

            municipality!.Name.Should().Be(_request.Name);
        }
    }
}
