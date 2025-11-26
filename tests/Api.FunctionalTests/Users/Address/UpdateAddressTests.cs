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
    }
}
