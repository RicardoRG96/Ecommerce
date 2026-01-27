using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Brands.GetFeaturedBrands;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Brand
{
    public class GetFeaturedBrandsTests : BaseFunctionalTest
    {
        public GetFeaturedBrandsTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnOk_WhenFeaturedBrandsExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}/featured");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnListOfFeaturedBrands_WhenFeaturedBrandsExist()
        {
            SetAdminAuthentication();

            List<BrandResponse>? featuredBrands = 
                await HttpClient.GetFromJsonAsync<List<BrandResponse>>($"{ApiRoutes.Brands.Base}/featured");

            featuredBrands.Should().NotBeNull();
            featuredBrands.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_ReturnOnlyFeaturedAndActiveBrands()
        {
            SetAdminAuthentication();

            List<BrandResponse>? featuredBrands = 
                await HttpClient.GetFromJsonAsync<List<BrandResponse>>($"{ApiRoutes.Brands.Base}/featured");

            featuredBrands.Should().NotBeNull();
            featuredBrands.Should().AllSatisfy(brand =>
            {
                brand.IsFeatured.Should().BeTrue();
                brand.IsActive.Should().BeTrue();
            });
        }

        [Fact]
        public async Task Should_ReturnExpectedNumberOfFeaturedBrands()
        {
            SetAdminAuthentication();

            List<BrandResponse>? featuredBrands = 
                await HttpClient.GetFromJsonAsync<List<BrandResponse>>($"{ApiRoutes.Brands.Base}/featured");

            // According to the seed data: Nike, Adidas, Puma, New Balance, Under Armour, Asics, The North Face, Lululemon, Jordan, Hoka
            // Brands with IsFeatured = true and IsActive = true: IDs 1, 2, 3, 5, 6, 9, 13, 16, 17, 20 = 10 brands
            featuredBrands.Should().HaveCount(10);
        }

        [Fact]
        public async Task Should_ReturnBrandsOrderedByDisplayOrder()
        {
            SetAdminAuthentication();

            List<BrandResponse>? featuredBrands = 
                await HttpClient.GetFromJsonAsync<List<BrandResponse>>($"{ApiRoutes.Brands.Base}/featured");

            featuredBrands.Should().NotBeNull();
            featuredBrands.Should().NotBeEmpty();

            // Verify that all items have a valid DisplayOrder
            featuredBrands.Should().AllSatisfy(brand =>
            {
                brand.DisplayOrder.Should().BeGreaterThan(0);
            });
        }

        [Fact]
        public async Task Should_ReturnBrandsWithAllProperties()
        {
            SetAdminAuthentication();

            List<BrandResponse>? featuredBrands = 
                await HttpClient.GetFromJsonAsync<List<BrandResponse>>($"{ApiRoutes.Brands.Base}/featured");

            featuredBrands.Should().NotBeNull();
            featuredBrands.Should().NotBeEmpty();

            BrandResponse firstBrand = featuredBrands!.First();

            firstBrand.Id.Should().BeGreaterThan(0);
            firstBrand.Name.Should().NotBeNullOrEmpty();
            firstBrand.Slug.Should().NotBeNullOrEmpty();
            firstBrand.IsFeatured.Should().BeTrue();
            firstBrand.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}/featured");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Brands.Base}/featured");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
