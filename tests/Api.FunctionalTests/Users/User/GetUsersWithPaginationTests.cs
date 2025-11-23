using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace Api.FunctionalTests.Users.User
{
    public class GetUsersWithPaginationTests : BaseFunctionalTest
    {
        public GetUsersWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan_1()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{usersBaseUrl}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
