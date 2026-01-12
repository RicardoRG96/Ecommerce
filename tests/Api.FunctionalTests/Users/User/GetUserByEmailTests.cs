using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Users.GetByEmail;
using FluentAssertions;
using System.Net;
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
            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}/email/{Constants.NotExistingEmail}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_User_WhenEmailExists()
        {
            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"{usersBaseUrl}/email/juan.perez@mail.com");

            user.Should().NotBeNull();
        }
    }
}
