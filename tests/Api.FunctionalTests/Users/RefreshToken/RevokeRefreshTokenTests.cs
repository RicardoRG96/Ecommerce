using Api.FunctionalTests.Abstractions;
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
    }
}
