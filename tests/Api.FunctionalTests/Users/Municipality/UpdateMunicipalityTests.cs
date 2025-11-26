using Api.FunctionalTests.Abstractions;
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
            UpdateMunicipalityRequest invalidRequest = _request with { RegionId = 0 };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{municipalitiesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
