using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.Address
{
    public class DeleteAddressTests : BaseFunctionalTest
    {
        public DeleteAddressTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressIsDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/2500");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
