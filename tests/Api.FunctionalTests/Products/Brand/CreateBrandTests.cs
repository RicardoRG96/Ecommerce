using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Products.Brand.Create;

namespace Api.FunctionalTests.Products.Brand
{
    public class CreateBrandTests : BaseFunctionalTest
    {
        private static readonly CreateBrandRequest _request = new(
            Name: "Samsung",
            Description: "Leading global technology company specializing in electronics and appliances",
            LogoUrl: "https://example.com/logos/samsung.png",
            BannerUrl: "https://example.com/banners/samsung-banner.jpg",
            WebsiteUrl: "https://www.samsung.com",
            IsActive: true,
            IsFeatured: true,
            DisplayOrder: 1,
            MetaTitle: "Samsung - Electronics & Technology",
            MetaDescription: "Shop Samsung smartphones, TVs, appliances and more",
            MetaKeywords: "samsung, electronics, smartphones, technology");

        public CreateBrandTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameIsMissing()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with { Name = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenNameExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                Name = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDescriptionExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                Description = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLogoUrlExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                LogoUrl = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenBannerUrlExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                BannerUrl = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenWebsiteUrlExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                WebsiteUrl = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenDisplayOrderIsMissing()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with { DisplayOrder = 0 };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaTitleExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                MetaTitle = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaDescriptionExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                MetaDescription = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMetaKeywordsExceedsMaximumLength()
        {
            SetAdminAuthentication();

            CreateBrandRequest invalidRequest = _request with 
            { 
                MetaKeywords = Constants.ExceededMaximumLengthField 
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnConflict_WhenBrandNameAlreadyExists()
        {
            SetAdminAuthentication();

            // Nike already exists in the seed data
            CreateBrandRequest invalidRequest = _request with { Name = "Nike" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long brandId = await response.Content.ReadFromJsonAsync<long>();

            brandId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerUserAuthentication();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(brandsBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
