using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Roles.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Role
{
    public class GetRoleByIdTests : BaseFunctionalTest
    {
        private static readonly long _adminRoleId = 1;

        public GetRoleByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRoleIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{rolesBaseUrl}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_AndRole_WhenRoleIdExistsAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            RoleResponse? role = await HttpClient.GetFromJsonAsync<RoleResponse>($"{rolesBaseUrl}/{_adminRoleId}");

            role.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response =
                await HttpClient.GetAsync($"{rolesBaseUrl}/{_adminRoleId}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response =
                await HttpClient.GetAsync($"{rolesBaseUrl}/{_adminRoleId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
