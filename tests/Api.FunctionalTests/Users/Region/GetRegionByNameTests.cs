using Api.FunctionalTests.Abstractions;
using Application.Users.Regions.GetByName;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Region
{
    public class GetRegionByNameTests : BaseFunctionalTest
    {
        public GetRegionByNameTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenRegionNameDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}/name/noRegion");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Region_WhenRegionNameExists()
        {
            RegionResponse? region = await HttpClient.GetFromJsonAsync<RegionResponse>($"{regionsBaseUrl}/name/Coquimbo");

            region.Should().NotBeNull();
        }
    }
}
