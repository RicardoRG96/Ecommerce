using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.Region
{
    public class DeleteRegionTests : BaseFunctionalTest
    {
        public DeleteRegionTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRegionIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{regionsBaseUrl}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{regionsBaseUrl}/2500");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
