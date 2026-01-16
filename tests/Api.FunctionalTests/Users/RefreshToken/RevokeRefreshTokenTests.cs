using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.RefreshToken
{
    public class RevokeRefreshTokenTests : BaseFunctionalTest
    {
        public RevokeRefreshTokenTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{usersBaseUrl}/refresh-tokens/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]  
        public async Task Should_ReturnNotFound_WhenUserIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{usersBaseUrl}/refresh-tokens/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValidAndUserIdExists()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{usersBaseUrl}/refresh-tokens/1");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
