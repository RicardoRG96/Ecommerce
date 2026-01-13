using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Addresses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Address
{
    public class GetAddressByTitleTests : BaseFunctionalTest
    {
        private readonly AddressHelper _addressHelper;

        public GetAddressByTitleTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _addressHelper = new AddressHelper(factory);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressTitleDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/title/{Constants.NotExistingTitle}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Address_WhenAddressTitleExistsAndUserIsLoggesIn()
        {
            SetAdminAuthentication();

            string addressTitle = await _addressHelper.GetAddressTitleForAdminUser();

            AddressResponse? address = await HttpClient.GetFromJsonAsync<AddressResponse>($"{addressesBaseUrl}/title/{addressTitle}");

            address.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedId()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/title/Casa 1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnInternalServerError_WhenTheLoggedInUserId_DoesNotMatchTheOneSent()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/title/Casa 1");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
