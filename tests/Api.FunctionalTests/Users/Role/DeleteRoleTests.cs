using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.Role
{
    public class DeleteRoleTests : BaseFunctionalTest
    {
        public DeleteRoleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenRoleIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{rolesBaseUrl}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
