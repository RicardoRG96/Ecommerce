using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Users.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.User
{
    public class GetUserByIdTests : BaseFunctionalTest
    {
        public GetUserByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenUserIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Users.Base}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_RetrurnOK_And_User_WhenUserExists()
        {
            SetAdminAuthentication();

            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"{ApiRoutes.Users.Base}/1");

            user.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Users.Base}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Users.Base}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
