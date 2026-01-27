using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Municipalities.GetWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Municipality
{
    public class GetMunicipalitiesWithPaginationTests : BaseFunctionalTest
    {
        public GetMunicipalitiesWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Municipalities_WhenPageNumber_And_PageSize_Values_AreCorrect()
        {
            SetAdminAuthentication();

            PaginatedList<MunicipalityResponse>? municipalities =
                await HttpClient.GetFromJsonAsync<PaginatedList<MunicipalityResponse>>($"{ApiRoutes.Locations.Municipalities}?pageNumber=1&pageSize=10");

            municipalities.Should().NotBeNull();
            municipalities.Items.Count.Should().Be(10);
            municipalities.TotalPages.Should().Be(3);
            municipalities.HasNextPage.Should().BeTrue();
            municipalities.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Locations.Municipalities}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
