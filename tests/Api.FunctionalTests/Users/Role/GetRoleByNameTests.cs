using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Roles.GetByName;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Role
{
    public class GetRoleByNameTests : BaseFunctionalTest
    {
        private static readonly string _adminRoleName = "Admin";

        public GetRoleByNameTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRoleNameDoesNotExist()
        {
            SetAdminAuthentication();

            string notExistingRoleName = "SuperAdmin";

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Admin.Roles}/name/{notExistingRoleName}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOK_AndRole_WhenRoleNameExistsAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            RoleResponse? role = await HttpClient.GetFromJsonAsync<RoleResponse>($"{ApiRoutes.Admin.Roles}/name/{_adminRoleName}");

            role.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response =
                await HttpClient.GetAsync($"{ApiRoutes.Admin.Roles}/name/{_adminRoleName}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response =
                await HttpClient.GetAsync($"{ApiRoutes.Admin.Roles}/name/{_adminRoleName}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
