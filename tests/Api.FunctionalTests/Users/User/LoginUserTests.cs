using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Create;
using Web.Api.Endpoints.v1.Users.User.Login;

namespace Api.FunctionalTests.Users.User
{
    public class LoginUserTests : BaseFunctionalTest
    {
        private static readonly CreateUserRequest _createUserRequest =
                new("", "TestName", "TestLastName", "Test", "test@example.com", "Test1234", new DateTime(2000, 10, 10), "+56923147859");

        private static readonly LoginUserRequest _request = new(_createUserRequest.Email, _createUserRequest.Password);

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
        public async Task Should_ReturnBadRequest_WhenEmailIsInvalid()
        {
            LoginUserRequest invalidRequest = _request with { Email = Constants.InvalidEmail };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenEmailExceedsTheMaximumLength()
        {
            LoginUserRequest invalidRequest =
                _request with { Email = Constants.ExceededMaximumLengthEmail };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordIsMissing()
        {
            LoginUserRequest invalidRequest = _request with { Password = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordExceedsTheMaximumLength()
        {
            LoginUserRequest invalidRequest =
                _request with { Password = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenUserDoesNotExist()
        {
            LoginUserRequest invalidRequest = _request with { Email = Constants.NotExistingEmail };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordDoesNotMatch()
        {
            await HttpClient.PostAsJsonAsync(usersBaseUrl, _request);

            LoginUserRequest invalidRequest = _request with { Password = "notMatching123" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
