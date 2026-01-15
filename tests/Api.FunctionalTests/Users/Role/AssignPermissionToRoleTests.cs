using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using Infrastructure.Access;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.AssignPermission;

namespace Api.FunctionalTests.Users.Role
{
    public class AssignPermissionToRoleTests : BaseFunctionalTest
    {
        private static readonly AssignPermissionToRoleRequest _request = 
            new(Roles.CustomerSupport, Permissions.Products.Read);

        public AssignPermissionToRoleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameIsMissing()
        {
            AssignPermissionToRoleRequest invalidRequest = _request with { RoleName = "" };

            HttpResponseMessage response = 
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/assign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameExceedsTheMaximumLength()
        {
            AssignPermissionToRoleRequest invalidRequest = 
                _request with { RoleName = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/assign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPermissionNameIsMissing()
        {
            AssignPermissionToRoleRequest invalidRequest = _request with { PermissionName = "" };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/assign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPermissionNameExceedsTheMaximumLength()
        {
            AssignPermissionToRoleRequest invalidRequest = 
                _request with { PermissionName = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/assign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRoleNameDoesNotExist()
        {
            AssignPermissionToRoleRequest invalidRequest = 
                _request with { PermissionName = Constants.NotExistingRoleName };

            HttpResponseMessage response =
                await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/permissions/assign", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
