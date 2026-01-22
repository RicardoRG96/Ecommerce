using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;

namespace Api.FunctionalTests.Products.Brand
{
    public class ActivateBrandTests : BaseFunctionalTest
    {
        public ActivateBrandTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task ShouldReturnBadRequest_WhenBrandIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PatchAsync($"{brandsBaseUrl}/0/activate", null!);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
