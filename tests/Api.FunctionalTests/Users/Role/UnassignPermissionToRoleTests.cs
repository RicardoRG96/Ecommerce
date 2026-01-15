using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using Infrastructure.Access;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.UnassignPermission;

namespace Api.FunctionalTests.Users.Role
{
    public class UnassignPermissionToRoleTests : BaseFunctionalTest
    {
        private static readonly UnassignPermissionToRoleRequest _request =
            new(Roles.CustomerSupport, Permissions.Products.Read);

        public UnassignPermissionToRoleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameIsMissing()
        {
            SetAdminAuthentication();

            UnassignPermissionToRoleRequest invalidRequest = _request with { RoleName = "" };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/unassign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UnassignPermissionToRoleRequest invalidRequest =
                _request with { RoleName = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/unassign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPermissionNameIsMissing()
        {
            SetAdminAuthentication();

            UnassignPermissionToRoleRequest invalidRequest = _request with { PermissionName = "" };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/unassign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPermissionNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UnassignPermissionToRoleRequest invalidRequest =
                _request with { PermissionName = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/unassign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRoleNameDoesNotExist()
        {
            SetAdminAuthentication();

            UnassignPermissionToRoleRequest invalidRequest =
                _request with { PermissionName = Constants.NotExistingRoleName };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/unassign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenPermissionNameDoesNotExist()
        {
            SetAdminAuthentication();

            UnassignPermissionToRoleRequest invalidRequest =
                _request with { PermissionName = Constants.NotExistingPermissionName };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/unassign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
