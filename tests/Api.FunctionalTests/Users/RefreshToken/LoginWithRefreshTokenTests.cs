using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.RefreshTokens.Login;
using Application.Users.Users.Login;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.RefreshToken.Login;

namespace Api.FunctionalTests.Users.RefreshToken
{
    public class LoginWithRefreshTokenTests : BaseFunctionalTest
    {
        private static readonly LoginWithRefreshTokenRequest _request = new("refreshToken");
        private readonly long _adminUserId;
        private readonly long _customerUserId;
        private readonly string _refreshToken;
        private readonly UsersHelper _usersHelper;

        public LoginWithRefreshTokenTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _usersHelper = new UsersHelper(factory);
            _adminUserId = AuthAdminUser.UserId;
            _customerUserId = AuthCustomerUser.UserId;
            _refreshToken = AuthAdminUser.RefreshToken;
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRefreshTokenIsMissing()
        {
            LoginWithRefreshTokenRequest invalidRequest = _request with { RefreshToken = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Users.Base}/refresh-tokens/1", invalidRequest);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Users.Base}/refresh-tokens/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRefreshTokenDoesNotExist()
        {
            LoginWithRefreshTokenRequest invalidRequest = _request with { RefreshToken = Constants.NotExistingRefreshToken };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Users.Base}/refresh-tokens/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRefreshTokenIsExpired()
        {
            LoginWithRefreshTokenRequest invalidRequest = _request with { RefreshToken = Constants.ExpiredRefreshToken };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Users.Base}/refresh-tokens/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnConflict_WhenRefreshTokenIsNotTheLatest()
        {
            LoginResponse? secondLoginResponse = await _usersHelper.LoginUser();

            LoginResponse? thirdLoginResponse = await _usersHelper.LoginUser();

            LoginWithRefreshTokenRequest firstTokenRequest = _request with { RefreshToken = secondLoginResponse!.RefreshToken };

            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", thirdLoginResponse!.AccessToken);

            HttpResponseMessage createRefreshTokenResponse = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Users.Base}/refresh-tokens/{_customerUserId}", firstTokenRequest);

            createRefreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Tokens_WhenRequestIsValid()
        {
            LoginWithRefreshTokenRequest firstTokenRequest = _request with { RefreshToken = _refreshToken };

            HttpResponseMessage createRefreshTokenResponse = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Users.Base}/refresh-tokens/{_adminUserId}", firstTokenRequest);

            RefreshTokenResponse? responseContent = await createRefreshTokenResponse.Content.ReadFromJsonAsync<RefreshTokenResponse>();

            responseContent!.AccessToken.Should().NotBeNull();
            responseContent.RefreshToken.Should().NotBeNull();
        }
    }
}
