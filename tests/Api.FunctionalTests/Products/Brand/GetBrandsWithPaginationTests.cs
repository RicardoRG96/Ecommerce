using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Brands.GetWithPagination;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Brand
{
    public class GetBrandsWithPaginationTests : BaseFunctionalTest
    {
        public GetBrandsWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Brands_WhenPageNumberAndPageSizeValuesAreCorrect()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=10");

            brands.Should().NotBeNull();
            brands!.Items.Should().NotBeEmpty();
            brands.Items.Count.Should().BeLessThanOrEqualTo(10);
        }

        [Fact]
        public async Task Should_ReturnCorrectTotalCount_WhenGettingFirstPage()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=10");

            brands.Should().NotBeNull();
            // According to the seed data there are 20 brands
            brands!.TotalCount.Should().Be(20);
        }

        [Fact]
        public async Task Should_ReturnCorrectTotalPages_WhenPageSizeIs_10()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=10");

            brands.Should().NotBeNull();
            // 20 brands / 10 per page = 2 pages
            brands!.TotalPages.Should().Be(2);
        }

        [Fact]
        public async Task Should_ReturnFirstPageOf_10Brands()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=10");

            brands.Should().NotBeNull();
            brands!.Items.Count.Should().Be(10);
            brands.PageNumber.Should().Be(1);
            brands.HasPreviousPage.Should().BeFalse();
            brands.HasNextPage.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnSecondPageOf_10Brands()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=2&pageSize=10");

            brands.Should().NotBeNull();
            brands!.Items.Count.Should().Be(10);
            brands.PageNumber.Should().Be(2);
            brands.HasPreviousPage.Should().BeTrue();
            brands.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnAllBrands_WhenPageSizeIs_100()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=100");

            brands.Should().NotBeNull();
            brands!.Items.Count.Should().Be(20);
            brands.TotalPages.Should().Be(1);
            brands.HasPreviousPage.Should().BeFalse();
            brands.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnEmptyList_WhenPageNumberExceedsTotalPages()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=100&pageSize=10");

            brands.Should().NotBeNull();
            brands!.Items.Should().BeEmpty();
            brands.TotalCount.Should().Be(20);
            brands.HasPreviousPage.Should().BeTrue();
            brands.HasNextPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnBrandsWithAllProperties()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? brands =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=5");

            brands.Should().NotBeNull();
            brands!.Items.Should().NotBeEmpty();

            BrandResponse firstBrand = brands.Items.First();
            firstBrand.Id.Should().BeGreaterThan(0);
            firstBrand.Name.Should().NotBeNullOrEmpty();
            firstBrand.Slug.Should().NotBeNullOrEmpty();
            firstBrand.DisplayOrder.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnDifferentBrands_OnDifferentPages()
        {
            SetAdminAuthentication();

            PaginatedList<BrandResponse>? firstPage =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=5");

            PaginatedList<BrandResponse>? secondPage =
                await HttpClient.GetFromJsonAsync<PaginatedList<BrandResponse>>($"{ApiRoutes.Brands.Base}?pageNumber=2&pageSize=5");

            firstPage.Should().NotBeNull();
            secondPage.Should().NotBeNull();

            // Verify that the brands from the first page are not in the second
            List<long> firstPageIds = firstPage!.Items.Select(b => b.Id).ToList();
            List<long> secondPageIds = secondPage!.Items.Select(b => b.Id).ToList();

            firstPageIds.Should().NotIntersectWith(secondPageIds);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
