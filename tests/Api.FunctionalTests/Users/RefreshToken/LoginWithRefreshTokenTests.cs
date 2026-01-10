using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.RefreshToken.Login;

namespace Api.FunctionalTests.Users.RefreshToken
{
    public class LoginWithRefreshTokenTests : BaseFunctionalTest
    {
        private static readonly LoginWithRefreshTokenRequest _request = new("refreshToken");

        public LoginWithRefreshTokenTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRefreshTokenIsMissing()
        {
            LoginWithRefreshTokenRequest invalidRequest = _request with { RefreshToken = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/refresh-tokens/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/refresh-tokens/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
