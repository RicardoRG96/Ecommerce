using Api.FunctionalTests.Abstractions;
using Application.Users.Regions.GetWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Regions_WhenPageNumber_And_PageSize_Values_AreCorrect()
        {
            SetAdminAuthentication();

            PaginatedList<RegionResponse>? regions =
                await HttpClient.GetFromJsonAsync<PaginatedList<RegionResponse>>($"{regionsBaseUrl}?pageNumber=1&pageSize=10");

            regions.Should().NotBeNull();
            regions.Items.Count.Should().Be(10);
            regions.TotalPages.Should().Be(2);
            regions.HasNextPage.Should().BeTrue();
            regions.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{regionsBaseUrl}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
