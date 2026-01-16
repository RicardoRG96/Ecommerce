using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Create;

namespace Api.FunctionalTests.Users.User
{
    public class CreateUserTests : BaseFunctionalTest
    {
        private static readonly CreateUserRequest _request = 
            new("", "TestName", "TestLastName", "Test", "test@example.com", "Test1234", new DateTime(2000, 10, 10), "+56923147859");

        public CreateUserTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenFirstNameIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { FirstName = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenFirstNameExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest = 
                _request with { FirstName = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLastNameIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { LastName = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenLastNameExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { LastName = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserNameIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { Username = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserNameExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { Username = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenEmailIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { Email = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenEmailIsInvalid()
        {
            CreateUserRequest invalidRequest = _request with { Email = Constants.InvalidEmail };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenEmailExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { Email = Constants.ExceededMaximumLengthEmail };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { Password = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { Password = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordHasNotTheMinumumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { Password = Constants.PasswordTooShort };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPhoneNumberIsMissing()
        {
            CreateUserRequest invalidRequest = _request with { PhoneNumber = "" };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPhoneNumberExceedsTheMaximumLength()
        {
            CreateUserRequest invalidRequest =
                _request with { PhoneNumber = Constants.ExceededMaximumLengthField };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnConflict_WhenEmailIsNotUnique()
        {
            CreateUserRequest invalidRequest = _request with { Email = Constants.NotUniqueEmail };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_ReturnConflict_WhenUserNameIsNotUnique()
        {
            CreateUserRequest invalidRequest = _request with { Username = Constants.NotUniqueUserName };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserHasNotLegalAge()
        {
            CreateUserRequest invalidRequest = _request with { DateOfBirth = new DateTime(2015, 10, 10) };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordDoesNotContainUppercase()
        {
            CreateUserRequest invalidRequest =
                _request with { Password = Constants.PasswordWithNoUppercase };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordDoesNotContainLowercase()
        {
            CreateUserRequest invalidRequest =
                _request with { Password = Constants.PasswordWithNoLowercase };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenPasswordDoesNotContainNumbers()
        {
            CreateUserRequest invalidRequest =
                _request with { Password = Constants.PasswordWithNoNumbers };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_WhenUserNameHasWhiteSpaces()
        {
            CreateUserRequest invalidRequest =
                _request with { Username = Constants.UserNameWithWithSpaces };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, invalidRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_ReturnOk_WhenRequestIsValid()
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(usersBaseUrl, _request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            long userId = await response.Content.ReadFromJsonAsync<long>();

            userId.Should().BeGreaterThan(0);
        }
    }
}
