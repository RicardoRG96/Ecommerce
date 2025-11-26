using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.Municipality
{
    public class DeleteMunicipalityTests : BaseFunctionalTest
    {
        public DeleteMunicipalityTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{municipalitiesBaseUrl}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
