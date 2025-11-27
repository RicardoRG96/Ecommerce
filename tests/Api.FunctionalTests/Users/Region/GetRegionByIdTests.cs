using Api.FunctionalTests.Abstractions;
using Application.Users.Regions.GetById;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Region
{
    public class GetRegionByIdTests : BaseFunctionalTest
    {
        public GetRegionByIdTests(FunctionalTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}/2500");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Region_WhenRegionIdExists()
        {
            RegionResponse? region = await HttpClient.GetFromJsonAsync<RegionResponse>($"{regionsBaseUrl}/1");

            region.Should().NotBeNull();
        }
    }
}
