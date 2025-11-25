using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Municipality.Create;

namespace Api.FunctionalTests.Users.Municipality
{
    public class CreateMunicipalityTests : BaseFunctionalTest
    {
        private static readonly CreateMunicipalityRequest _request = new(1, "TestMuni");

        public CreateMunicipalityTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionIdIsMissing()
        {
            CreateMunicipalityRequest invalidRequest = _request with { RegionId = 0 };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(municipalitiesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
