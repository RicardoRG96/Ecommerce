using Api.FunctionalTests.Abstractions;
using Application.Users.Addresses;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Address
{
    public class GetAddressesWithPaginationTests : BaseFunctionalTest
    {
        public GetAddressesWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan_1()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Addresses_WhenPageNumber_And_PageSize_Values_AreCorrect()
        {
            PaginatedList<AddressResponse>? addresses =
                await HttpClient.GetFromJsonAsync<PaginatedList<AddressResponse>>($"{addressesBaseUrl}?pageNumber=1&pageSize=10");

            addresses.Should().NotBeNull();
            addresses.Items.Count.Should().Be(10);
            addresses.TotalPages.Should().Be(2);
            addresses.HasNextPage.Should().BeTrue();
            addresses.HasPreviousPage.Should().BeFalse();
        }
    }
}
