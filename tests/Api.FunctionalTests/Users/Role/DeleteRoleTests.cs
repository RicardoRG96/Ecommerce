using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.Role
{
    public class DeleteRoleTests : BaseFunctionalTest
    {
        public DeleteRoleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{rolesBaseUrl}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRoleIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{rolesBaseUrl}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValidAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            long guestRoleId = 7;

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{rolesBaseUrl}/{guestRoleId}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
