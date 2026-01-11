using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Users.Login;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using Web.Api.Endpoints.v1.Users.RefreshToken.Login;

namespace Api.FunctionalTests.Users.RefreshToken
{
    public class LoginWithRefreshTokenTests : BaseFunctionalTest
    {
        private static readonly LoginWithRefreshTokenRequest _request = new("refreshToken");
        private readonly UsersHelper _usersHelper;

        public LoginWithRefreshTokenTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _usersHelper = new UsersHelper(factory);
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

        [Fact]
        public async Task Should_ReturnNotFound_WhenRefreshTokenDoesNotExist()
        {
            LoginWithRefreshTokenRequest invalidRequest = _request with { RefreshToken = Constants.NotExistingRefreshToken };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/refresh-tokens/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRefreshTokenIsExpired()
        {
            LoginWithRefreshTokenRequest invalidRequest = _request with { RefreshToken = Constants.ExpiredRefreshToken };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/refresh-tokens/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnConflict_WhenRefreshTokenIsNotTheLatest()
        {
            long userId = await _usersHelper.CreateUser();

            UserResponse? tokens = await _usersHelper.LoginUser();

            LoginWithRefreshTokenRequest firstTokenRequest = _request with { RefreshToken = tokens!.RefreshToken };

            await _usersHelper.LoginUser();

            HttpResponseMessage createRefreshTokenResponse = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/refresh-tokens/{userId}", firstTokenRequest);

            createRefreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_ReturnOK_WhenRequestIsValid()
        {
            long userId = await _usersHelper.CreateUser();

            UserResponse? tokens = await _usersHelper.LoginUser();

            LoginWithRefreshTokenRequest firstTokenRequest = _request with { RefreshToken = tokens!.RefreshToken };

            HttpResponseMessage createRefreshTokenResponse = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/refresh-tokens/{userId}", firstTokenRequest);

            createRefreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
