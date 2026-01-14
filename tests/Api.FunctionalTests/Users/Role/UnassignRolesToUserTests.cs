using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.AssignRoles;
using Web.Api.Endpoints.v1.Users.Role.UnassignRoles;

namespace Api.FunctionalTests.Users.Role
{
    public class UnassignRolesToUserTests : BaseFunctionalTest
    {
        private readonly UnassignRolesToUserRequest _request;
        private readonly RoleHelper _roleHelper = new();

        public UnassignRolesToUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _request = new(_roleHelper.GetRoleNames());
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRolesAreEmpty()
        {
            SetAdminAuthentication();

            UnassignRolesToUserRequest invalidRequest = _request with { Roles = [] };

            HttpResponseMessage response = 
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/unassign/user/{AuthAdminUser.UserId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRolesItemsAreEmpty()
        {
            SetAdminAuthentication();

            UnassignRolesToUserRequest invalidRequest = _request with { Roles = ["", ""] };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/unassign/user/{AuthAdminUser.UserId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
