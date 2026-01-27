using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Brand.Update;

namespace Api.FunctionalTests.Products.Brand
{
    public class UpdateBrandTests : BaseFunctionalTest
    {
        private static readonly UpdateBrandRequest _request = new(
            Name: "Sony Updated",
            Description: "Updated description for Sony electronics and entertainment company",
            LogoUrl: "https://example.com/logos/sony-updated.png",
            BannerUrl: "https://example.com/banners/sony-updated.jpg",
            WebsiteUrl: "https://www.sony.com",
            IsActive: true,
            IsFeatured: true,
            DisplayOrder: 5,
            MetaTitle: "Sony - Electronics & Entertainment Updated",
            MetaDescription: "Shop Sony cameras, TVs, PlayStation and more - Updated",
            MetaKeywords: "sony, electronics, playstation, cameras, updated");

        public UpdateBrandTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenBrandIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameIsMissing()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                Name = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDescriptionExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                Description = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLogoUrlExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                LogoUrl = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenBannerUrlExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                BannerUrl = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenWebsiteUrlExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                WebsiteUrl = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDisplayOrderIsMissing()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with { DisplayOrder = 0 };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaTitleExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                MetaTitle = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaDescriptionExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                MetaDescription = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaKeywordsExceedsMaximumLength()
        {
            SetAdminAuthentication();

            UpdateBrandRequest invalidRequest = _request with 
            { 
                MetaKeywords = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenBrandIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnConflict_WhenBrandNameAlreadyExists()
        {
            SetAdminAuthentication();

            // Nike is the name of the brand with ID 1 in the seed data
            UpdateBrandRequest invalidRequest = _request with { Name = "Nike" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/2", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid_And_BrandIdExists()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/2", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Brands.Base}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
