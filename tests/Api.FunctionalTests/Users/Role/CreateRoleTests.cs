using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.Create;

namespace Api.FunctionalTests.Users.Role
{
    public class CreateRoleTests : BaseFunctionalTest
    {
        private static readonly CreateRoleRequest _request = new("InventoryManager");

        public CreateRoleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameIsMissing()
        {
            SetAdminAuthentication();

            CreateRoleRequest invalidRequest = _request with { RoleName = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleNameExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            CreateRoleRequest invalidRequest = _request with { RoleName = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnConflict_WhenRoleAlreadyExists()
        {
            SetAdminAuthentication();

            CreateRoleRequest invalidRequest = _request with { RoleName = Constants.AlreadyExistingRole };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_ReturnOK_WhenRequestIsValidAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{rolesBaseUrl}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
