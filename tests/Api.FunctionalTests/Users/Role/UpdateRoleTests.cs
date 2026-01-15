using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.Update;

namespace Api.FunctionalTests.Users.Role
{
    public class UpdateRoleTests : BaseFunctionalTest
    {
        private static readonly UpdateRoleRequest _request = new("GuestRole");
        private static readonly long _GuestRoleId = 7;

        public UpdateRoleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{rolesBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameIsMissing()
        {
            SetAdminAuthentication();

            UpdateRoleRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{rolesBaseUrl}/{_GuestRoleId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateRoleRequest invalidRequest = _request with { Name = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{rolesBaseUrl}/{_GuestRoleId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRoleIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{rolesBaseUrl}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNocontent_WhenRequestIsValidAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{rolesBaseUrl}/{_GuestRoleId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
