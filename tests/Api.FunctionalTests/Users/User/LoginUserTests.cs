using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Login;

namespace Api.FunctionalTests.Users.User
{
    public class LoginUserTests : BaseFunctionalTest
    {
        private static readonly LoginUserRequest _request = new("test@example.com", "Test1234");

        public LoginUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenEmailIsMissing()
        {
            LoginUserRequest invalidRequest = _request with { Email = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_BadRequest_WhenEmailIsInvalid()
        {
            LoginUserRequest invalidRequest = _request with { Email = Constants.InvalidEmail };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
