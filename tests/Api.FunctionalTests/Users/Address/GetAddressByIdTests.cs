using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Addresses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Address
{
    public class GetAddressByIdTests : BaseFunctionalTest
    {
        private readonly AddressHelper _addressHelper;

        public GetAddressByIdTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _addressHelper = new AddressHelper(factory);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/{Constants.NotExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_And_Address_WhenAddressIdExistsAndUserIsLoggedIn()
        {
            SetAdminAuthentication();

            long createdAddressId = await _addressHelper.CreateAddressForAdminUser();

            AddressResponse? address = await HttpClient.GetFromJsonAsync<AddressResponse>($"{addressesBaseUrl}/{createdAddressId}");

            address.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedId()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/1");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnInternalServerError_WhenTheLoggedInUserId_DoesNotMatchTheOneSent()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}/1");

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
