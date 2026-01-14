using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.AssignRoles;

namespace Api.FunctionalTests.Users.Role
{
    public class AssignRolesToUserTests : BaseFunctionalTest
    {
        private readonly AssignRolesToUserRequest _request;
        private readonly RoleHelper _roleHelper = new();

        public AssignRolesToUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _request = new(_roleHelper.GetRoleNames());
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRolesAreEmpty()
        {
            SetAdminAuthentication();

            AssignRolesToUserRequest invalidRequest = _request with { Roles = [] };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/22", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRolesItemsAreEmpty()
        {
            SetAdminAuthentication();

            AssignRolesToUserRequest invalidRequest = _request with { Roles = ["", ""] };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/22", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenUserIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/200", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValidAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/{AuthAdminUser.UserId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/{AuthAdminUser.UserId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/{AuthCustomerUser.UserId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
