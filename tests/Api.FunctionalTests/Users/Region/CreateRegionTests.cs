using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Region.Create;

namespace Api.FunctionalTests.Users.Region
{
    public class CreateRegionTests : BaseFunctionalTest
    {
        private static readonly CreateRegionRequest _request = new("TestRegion");

        public CreateRegionTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameIsMissing()
        {
            CreateRegionRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionNameExceedsTheMaximumLength()
        {
            CreateRegionRequest invalidRequest =
                _request with { Name = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(regionsBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long regionId = await response.Content.ReadFromJsonAsync<long>();

            regionId.Should().BeGreaterThan(0);
        }
    }
}
