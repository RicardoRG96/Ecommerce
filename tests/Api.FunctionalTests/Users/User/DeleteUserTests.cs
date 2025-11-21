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

        [Fact]
        public async Task Should_ReturnNotFound_WhenUserDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"api/v1/users/2500");
            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenCountryExists()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("api/v1/countries/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
