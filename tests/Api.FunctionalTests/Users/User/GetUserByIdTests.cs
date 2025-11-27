using Api.FunctionalTests.Abstractions;
using Application.Users.Users.GetById;
using FluentAssertions;
using System.Net;
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
            long notExistingUserId = 2500;

            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}/{notExistingUserId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_RetrurnOK_And_User_WhenUserExists()
        {
            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"{usersBaseUrl}/1");

            user.Should().NotBeNull();
        }
    }
}
