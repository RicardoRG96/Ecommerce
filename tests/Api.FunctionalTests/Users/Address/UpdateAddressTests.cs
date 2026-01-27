using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Users.Addresses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Address.Update;

namespace Api.FunctionalTests.Users.Address
{
    public class UpdateAddressTests : BaseFunctionalTest
    {
        private readonly AddressHelper _addressHelper;

        private static readonly UpdateAddressRequest _request = new(
            1, 
            "UpdatedTitle", 
            "UpdatedCity", 
            "UpdatedStreet", 
            "UpdatedNumber", 
            "UpdatedApartament", 
            "UpdatedReference", 
            "UpdatedPostalCode");

        public UpdateAddressTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _addressHelper = new AddressHelper(factory);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressIdIsMissing()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityIdIsMissing()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest = _request with { MunicipalityId = 0 };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressTitleIsMissing()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest = _request with { Title = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressTitleExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest =
                _request with { Title = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressCityIsMissing()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest = _request with { City = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressCityExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest =
                _request with { City = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressStreetIsMissing()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest = _request with { Street = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressStreetExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest =
                _request with { Street = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressNumberIsMissing()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest = _request with { Number = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressNumberExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest =
                _request with { Number = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressApartamentExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest =
                _request with { Apartament = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressReferenceExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest =
                _request with { Reference = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressPostalCodeIsMissing()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest = _request with { PostalCode = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressPostalCodeExceedsTheMaximumLength()
        {
            SetAdminAuthentication();

            UpdateAddressRequest invalidRequest =
                _request with { PostalCode = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressIdDoesNotExist()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/{Constants.NotExistingId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            SetAdminAuthentication();

            long createdAddressId = await _addressHelper.CreateAddressForAdminUserAsync();

            UpdateAddressRequest invalidRequest = _request with { MunicipalityId = Constants.NotExistingId };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/{createdAddressId}", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNoContent_WhenRequestIsValid_AddressIdExists_AndUserIsLoggedId()
        {
            SetAdminAuthentication();

            long createdAddressId = await _addressHelper.CreateAddressForAdminUserAsync();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/{createdAddressId}", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_UpdateTitle_WhenRequestIsValid_And_AddresIdExists()
        {
            SetAdminAuthentication();

            long createdAddressId = await _addressHelper.CreateAddressForAdminUserAsync();

            await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/{createdAddressId}", _request);

            AddressResponse? address = await HttpClient.GetFromJsonAsync<AddressResponse>($"{ApiRoutes.Addresses.Base}/{createdAddressId}");

            address!.Title.Should().Be(_request.Title);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            SetCustomerSupportUserAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_ReturnInternalServerError_WhenTheLoggedInUserId_DoesNotMatchTheOneSent()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{ApiRoutes.Addresses.Base}/1", _request);

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
