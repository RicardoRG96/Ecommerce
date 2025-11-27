using Api.FunctionalTests.Abstractions;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.Address.Create;
using Web.Api.Endpoints.v1.Users.Address.Update;

namespace Api.FunctionalTests.Users.Address
{
    public class UpdateAddressTests : BaseFunctionalTest
    {
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
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressIdIsMissing()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/0", _request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenMunicipalityIdIsMissing()
        {
            UpdateAddressRequest invalidRequest = _request with { MunicipalityId = 0 };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressTitleIsMissing()
        {
            UpdateAddressRequest invalidRequest = _request with { Title = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressTitleExceedsTheMaximumLength()
        {
            UpdateAddressRequest invalidRequest =
                _request with { Title = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressCityIsMissing()
        {
            UpdateAddressRequest invalidRequest = _request with { City = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressCityExceedsTheMaximumLength()
        {
            UpdateAddressRequest invalidRequest =
                _request with { City = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressStreetIsMissing()
        {
            UpdateAddressRequest invalidRequest = _request with { Street = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressStreetExceedsTheMaximumLength()
        {
            UpdateAddressRequest invalidRequest =
                _request with { Street = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressNumberIsMissing()
        {
            UpdateAddressRequest invalidRequest = _request with { Number = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressNumberExceedsTheMaximumLength()
        {
            UpdateAddressRequest invalidRequest =
                _request with { Number = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressApartamentExceedsTheMaximumLength()
        {
            UpdateAddressRequest invalidRequest =
                _request with { Apartament = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressReferenceExceedsTheMaximumLength()
        {
            UpdateAddressRequest invalidRequest =
                _request with { Reference = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressPostalCodeIsMissing()
        {
            UpdateAddressRequest invalidRequest = _request with { PostalCode = "" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenAddressPostalCodeExceedsTheMaximumLength()
        {
            UpdateAddressRequest invalidRequest =
                _request with { PostalCode = "La República Federal del Crisantemo Esmeralda de los Montes Orientales del Viento" };

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/1", invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnNotFound_WhenAddressIdDoesNotExist()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{addressesBaseUrl}/2500", _request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
