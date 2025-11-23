using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.User
{
    public class GetUserByEmailTests : BaseFunctionalTest
    {
        public GetUserByEmailTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenEmailDoesNotExist()
        {
            string notExistingEmail = "no_email@example.com";

            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}/email/{notExistingEmail}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
