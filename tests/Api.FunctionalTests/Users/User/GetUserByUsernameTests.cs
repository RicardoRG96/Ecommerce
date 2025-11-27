using Api.FunctionalTests.Abstractions;
using Application.Users.Users.GetByUsername;
using FluentAssertions;
using System.Net;
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
            string notExistingUsername = "notExistingUser";

            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}/username/{notExistingUsername}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_User_WhenUsernameExists()
        {
            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"{usersBaseUrl}/username/Ricardor");

            user.Should().NotBeNull();
        }
    }
}
