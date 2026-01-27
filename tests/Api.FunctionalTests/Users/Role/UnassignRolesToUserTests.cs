using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.AssignRoles;
using Web.Api.Endpoints.v1.Users.Role.UnassignRoles;

namespace Api.FunctionalTests.Users.Role
{
    public class UnassignRolesToUserTests : BaseFunctionalTest
    {
        private readonly UnassignRolesToUserRequest _request;
        private readonly AssignRolesToUserRequest _assignRolesRequest;
        private readonly RoleHelper _roleHelper = new();

        public UnassignRolesToUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _request = new(_roleHelper.GetRoleNames());
            _assignRolesRequest = new(_roleHelper.GetRoleNames());
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRolesAreEmpty()
        {
            SetAdminAuthentication();

            UnassignRolesToUserRequest invalidRequest = _request with { Roles = [] };

            HttpResponseMessage response = 
                await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/unassign/user/{AuthAdminUser.UserId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRolesItemsAreEmpty()
        {
            SetAdminAuthentication();

            UnassignRolesToUserRequest invalidRequest = _request with { Roles = ["", ""] };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/unassign/user/{AuthAdminUser.UserId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/unassign/user/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenUserIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/unassign/user/200", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValidAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            var assingRolesToUser = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/assign/user/{AuthAdminUser.UserId}", _assignRolesRequest);

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/unassign/user/{AuthAdminUser.UserId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/unassign/user/{AuthAdminUser.UserId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{ApiRoutes.Admin.Roles}/unassign/user/{AuthCustomerUser.UserId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
