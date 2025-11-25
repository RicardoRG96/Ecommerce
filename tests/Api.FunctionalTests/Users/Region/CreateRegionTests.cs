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
    }
}
