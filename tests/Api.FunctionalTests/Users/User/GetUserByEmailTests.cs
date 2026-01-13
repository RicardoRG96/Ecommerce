using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Users.GetByEmail;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.User
{
    public class GetUserByEmailTests : BaseFunctionalTest
    {
        public GetUserByEmailTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenEmailDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}/email/{Constants.NotExistingEmail}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_User_WhenEmailExists()
        {
            SetAdminAuthentication();

            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"{usersBaseUrl}/email/juan.perez@mail.com");

            user.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}/email/juan.perez@mail.com");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}/email/juan.perez@mail.com");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
