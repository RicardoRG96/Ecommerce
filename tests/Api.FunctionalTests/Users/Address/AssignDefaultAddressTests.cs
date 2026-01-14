using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Address.AssignDefaultAddress;

namespace Api.FunctionalTests.Users.Address
{
    public class AssignDefaultAddressTests : BaseFunctionalTest
    {
        private readonly AddressHelper _addressHelper;
        private long _createdAddressId;

        public AssignDefaultAddressTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _addressHelper = new AddressHelper(factory);
        }

        private async Task SetCreatedAddressIdAsync()
        {
            _createdAddressId = await _addressHelper.CreateAddressForCustomerUserAsync();
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserIdIsMissing()
        {
            AssignDefaultAddressRequest request = new(default);

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/me/default-address/0", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
