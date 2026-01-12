using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Users.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Update;

namespace Api.FunctionalTests.Users.User
{
    public class UpdateUserTests : BaseFunctionalTest
    {
        private static readonly UpdateUserRequest _request = new("user1.jpg", "Ricardo", "Guerrero", "+56912121212");

        public UpdateUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenFirstNameIsMissing()
        {
            UpdateUserRequest invalidRequest = _request with { FirstName = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLastNameIsMissing()
        {
            UpdateUserRequest invalidRequest = _request with { LastName = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPhoneNumberIsMissing()
        {
            UpdateUserRequest invalidRequest = _request with { PhoneNumber = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenUserIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenUserIdExists()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateLastName_WhenUserIdExistsAndRequestIsValid()
        {
            await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/1", _request);

            UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"{usersBaseUrl}/1");

            user!.LastName.Should().Be(_request.LastName);
        }
    }
}
