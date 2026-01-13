using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Address.Create;

namespace Api.FunctionalTests.Users.Address
{
    public class DeleteAddressTests : BaseFunctionalTest
    {
        public DeleteAddressTests(FunctionalTestWebAppFactory factory)
            : base(factory)
        {
        }

        private async Task<long> CreateAddressForAdminUser()
        {
           CreateAddressRequest request =
                new(1, 1, "AdminAddress", "TestCity", "TestStreet", "1010", "TestApartament", "TestReference", "TestPostalCode");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, request);

            return await response.Content.ReadFromJsonAsync<long>();
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressIsDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenAddressIdExists()
        {
            SetAdminAuthentication();

            long createdAdderssId = await CreateAddressForAdminUser();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/{createdAdderssId}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnInternalServerError_WhenTheLoggedInUserId_DoesNotMatchTheOneSent()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/1");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
