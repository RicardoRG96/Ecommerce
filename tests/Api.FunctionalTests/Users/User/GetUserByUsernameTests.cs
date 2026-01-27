using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Users.GetByUsername;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.User
{
    public class GetUserByUsernameTests : BaseFunctionalTest
    {
        public GetUserByUsernameTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenUsernameDoesNotExist()
        {
            SetAdminAuthentication();

            string notExistingUsername = "notExistingUser";

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Users.Base}/username/{notExistingUsername}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_User_WhenUsernameExists()
        {
            SetAdminAuthentication();

            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"{ApiRoutes.Users.Base}/username/maria.gonzalez");

            user.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Users.Base}/username/maria.gonzalez");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Users.Base}/username/maria.gonzalez");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
