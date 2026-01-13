using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Address.Create;

namespace Api.FunctionalTests.Users.Address
{
    public class CreateAddressTests : BaseFunctionalTest
    {
        private static readonly CreateAddressRequest _request = 
            new(1, 1, "TestAddress", "TestCity", "TestStreet", "TestNumber", "TestApartament", "TestReference", "TestPostalCode");

        public CreateAddressTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        private void ApplyBearerToken()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", AuthAdminUser.AccessToken);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenCountryIdIsMissing()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { CountryId = 0 };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityIdIsMissing()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { MunicipalityId = 0 };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressTitleIsMissing()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Title = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressTitleExceedsTheMaximumLength()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Title = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressCityIsMissing()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { City = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressCityExceedsTheMaximumLength()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { City = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressStreetIsMissing()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Street = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressStreetExceedsTheMaximumLength()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Street = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressNumberIsMissing()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Number = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressNumberExceedsTheMaximumLength()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Number = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressApartamentExceedsTheMaximumLength()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Apartament = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressReferenceExceedsTheMaximumLength()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { Reference = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressPostalCodeIsMissing()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { PostalCode = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressPostalCodeExceedsTheMaximumLength()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest =
                _request with { PostalCode = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenCountryIdDoesNotExist()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { CountryId = Constants.NotExistingId };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenMunicipalityIdDoesNotExist()
        {
            ApplyBearerToken();

            CreateAddressRequest invalidRequest = _request with { MunicipalityId = Constants.NotExistingId };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            ApplyBearerToken();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long addressId = await response.Content.ReadFromJsonAsync<long>();

            addressId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(addressesBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
