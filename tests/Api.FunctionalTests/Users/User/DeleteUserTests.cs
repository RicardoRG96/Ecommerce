using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.User
{
    public class DeleteUserTests : BaseFunctionalTest
    {
        public DeleteUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync("api/v1/users/");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
