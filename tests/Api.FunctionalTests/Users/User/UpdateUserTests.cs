using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Update;
using Web.Api.Infrastructure;

namespace Api.FunctionalTests.Users.User
{
    public class UpdateUserTests : BaseFunctionalTest
    {
        private static readonly UpdateUserRequest request = new("user1.jpg", "Ricardo", "Guerrero", "+56912121212");

        public UpdateUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("api/v1/users/0", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenFirstNameIsMissing()
        {
            UpdateUserRequest invalidRequest = request with { FirstName = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("api/v1/users/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLastNameIsMissing()
        {
            UpdateUserRequest invalidRequest = request with { LastName = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("api/v1/users/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPhoneNumberIsMissing()
        {
            UpdateUserRequest invalidRequest = request with { PhoneNumber = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("api/v1/users/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
