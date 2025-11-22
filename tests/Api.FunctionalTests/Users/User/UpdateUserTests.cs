using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User;

namespace Api.FunctionalTests.Users.User
{
    public class UpdateUserTests : BaseFunctionalTest
    {
        public UpdateUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            Update.Request request = new("user1.jpg", "Ricardo", "Guerrero", "+56912121212");

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("api/v1/users/0", request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
