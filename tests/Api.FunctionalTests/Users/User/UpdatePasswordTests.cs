using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.UpdatePassword;

namespace Api.FunctionalTests.Users.User
{
    public class UpdatePasswordTests : BaseFunctionalTest
    {
        private static readonly UpdatePasswordRequest _request = new("Test1234", "Test.123");

        public UpdatePasswordTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/me/password/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCurrentPasswordIsMissing()
        {
            SetCustomerUserAuthentication();

            UpdatePasswordRequest invalidRequest = _request with { CurrentPassword = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/me/password/{AuthCustomerUser.UserId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCurrentPasswordExceedsTheMaximumLength()
        {
            SetCustomerUserAuthentication();

            UpdatePasswordRequest invalidRequest = _request with { CurrentPassword = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{usersBaseUrl}/me/password/{AuthCustomerUser.UserId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
