using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

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
            HttpResponseMessage response = await HttpClient.DeleteAsync($"api/v1/users/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
