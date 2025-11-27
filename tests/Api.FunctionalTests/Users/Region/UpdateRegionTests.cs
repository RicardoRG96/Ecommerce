using Api.FunctionalTests.Abstractions;
using Application.Users.Regions.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Region.Update;

namespace Api.FunctionalTests.Users.Region
{
    public class UpdateRegionTests : BaseFunctionalTest
    {
        private static readonly UpdateRegionRequest _request = new("UpdatedName");

        public UpdateRegionTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameIsMissing()
        {
            UpdateRegionRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameExceedsTheMaximumLength()
        {
            UpdateRegionRequest invalidRequest =
                _request with { Name = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/2500", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]  
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_RegionIdExists()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateName_WhenRequestIsValid_And_RegionIdExists()
        {
            await HttpClient.PutAsJsonAsync($"{regionsBaseUrl}/1", _request);

            RegionResponse? region = await HttpClient.GetFromJsonAsync<RegionResponse>($"{regionsBaseUrl}/1");

            region!.Name.Should().Be(_request.Name);
        }
    }
}
