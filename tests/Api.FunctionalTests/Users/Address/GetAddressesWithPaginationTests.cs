using Api.FunctionalTests.Abstractions;
using Application.Users.Addresses;
using FluentAssertions;
using SharedKernel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users.Address
{
    public class GetAddressesWithPaginationTests : BaseFunctionalTest
    {
        private readonly AddressHelper _addressHelper;

        public GetAddressesWithPaginationTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
            _addressHelper = new AddressHelper(factory);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageNumberIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}?pageNumber=0&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsLessThan_1()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}?pageNumber=1&pageSize=0");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPageSizeIsGreaterThan_100()
        {
            SetAdminAuthentication();

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}?pageNumber=1&pageSize=101");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOK_And_Addresses_WhenPageNumber_And_PageSize_Values_AreCorrectAndUserIsLoggedId()
        {
            SetAdminAuthentication();

            await _addressHelper.CreateAddressForAdminUser();

            PaginatedList<AddressResponse>? addresses =
                await HttpClient.GetFromJsonAsync<PaginatedList<AddressResponse>>($"{addressesBaseUrl}?pageNumber=1&pageSize=10");

            addresses.Should().NotBeNull();
            addresses.Items.Count.Should().Be(1);
            addresses.TotalPages.Should().Be(1);
            addresses.HasNextPage.Should().BeFalse();
            addresses.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnOK_And_EmptyAddresses_WhenTheLoggedInUserId_DoesNotHaveAddresses()
        {
            SetCustomerUserAuthentication();

            PaginatedList<AddressResponse>? addresses =
                await HttpClient.GetFromJsonAsync<PaginatedList<AddressResponse>>($"{addressesBaseUrl}?pageNumber=1&pageSize=10");

            addresses.Should().NotBeNull();
            addresses.Items.Count.Should().Be(0);
            addresses.TotalPages.Should().Be(0);
            addresses.HasNextPage.Should().BeFalse();
            addresses.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedId()
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            HttpResponseMessage response = await HttpClient.GetAsync($"{addressesBaseUrl}?pageNumber=1&pageSize=10");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
