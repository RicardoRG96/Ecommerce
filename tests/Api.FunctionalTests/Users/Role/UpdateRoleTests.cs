using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Role.Update;

namespace Api.FunctionalTests.Users.Role
{
    public class UpdateRoleTests : BaseFunctionalTest
    {
        private static readonly UpdateRoleRequest _request = new("GuestRole");

        public UpdateRoleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{rolesBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
