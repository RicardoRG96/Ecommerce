using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
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
        public async Task Should_ReturnInternalServerError_WhenTheLoggedInUserId_DoesNotMatchTheOneSent()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}/200", _request);

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
