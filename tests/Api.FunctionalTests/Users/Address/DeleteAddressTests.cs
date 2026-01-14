using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;

namespace Api.FunctionalTests.Users.Address
{
    public class DeleteAddressTests : BaseFunctionalTest
    {
        private readonly AddressHelper _addressHelper;

        public DeleteAddressTests(FunctionalTestWebAppFactory factory)
            : base(factory)
        {
            _addressHelper = new AddressHelper(factory);
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

            long createdAdderssId = await _addressHelper.CreateAddressForAdminUserAsync();

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

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerSupportUserAuthentication();

            HttpResponseMessage response = await HttpClient.DeleteAsync($"{addressesBaseUrl}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
