using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Create;

namespace Api.FunctionalTests.Users.User
{
    public class CreateUserTests : BaseFunctionalTest
    {
        private static readonly CreateUserRequest _request = 
            new("", "TestName", "TestLastName", "Test", "test@example.com", "Test1234", new DateTime(2000, 10, 10), "+56923147859");

        public CreateUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenFirstNameIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { FirstName = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenFirstNameExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest = 
                _request with { FirstName = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLastNameIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { LastName = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLastNameExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { LastName = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserNameIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { Username = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserNameExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { Username = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenEmailIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { Email = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
