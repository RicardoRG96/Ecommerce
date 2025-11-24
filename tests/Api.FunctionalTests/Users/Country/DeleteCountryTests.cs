using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.Country
{
    public class DeleteCountryTests : BaseFunctionalTest
    {
        public DeleteCountryTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{countriesBaseUrl}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
