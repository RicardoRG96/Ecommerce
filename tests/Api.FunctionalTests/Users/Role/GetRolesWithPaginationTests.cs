using Api.FunctionalTests.Abstractions;
using Application.Users.Roles.GetWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Role
{
    public class GetRolesWithPaginationTests : BaseFunctionalTest
    {
        public GetRolesWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIslessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{rolesBaseUrl}/?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{rolesBaseUrl}/?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{rolesBaseUrl}/?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Roles_WhenPageNumber_And_PageSize_Values_AreCorrect()
        {
            SetAdminAuthentication();

            PaginatedList<RoleResponse>? roles =
                await HttpClient.GetFromJsonAsync<PaginatedList<RoleResponse>>($"{rolesBaseUrl}/?pageNumber=2&pageSize=3");

            roles.Should().NotBeNull();
            roles.Items.Count.Should().Be(3);
            roles.TotalPages.Should().Be(3);
            roles.HasNextPage.Should().BeTrue();
            roles.HasPreviousPage.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{rolesBaseUrl}/?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{rolesBaseUrl}/?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
