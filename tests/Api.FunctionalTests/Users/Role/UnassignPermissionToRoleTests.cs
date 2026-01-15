using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using Infrastructure.Access;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.AssignPermission;
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
    }
}
