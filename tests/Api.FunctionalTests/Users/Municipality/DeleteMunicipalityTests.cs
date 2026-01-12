using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
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

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{municipalitiesBaseUrl}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenMunicipalityIdExists()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{municipalitiesBaseUrl}/21");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
