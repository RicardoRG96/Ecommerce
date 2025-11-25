using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Users.Region
{
    public class GetRegionsWithPaginationTests : BaseFunctionalTest
    {
        public GetRegionsWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan_1()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
