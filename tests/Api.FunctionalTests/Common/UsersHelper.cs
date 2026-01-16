using Api.FunctionalTests.Abstractions;
using Application.Users.Users.Login;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Create;
using Web.Api.Endpoints.v1.Users.User.Login;

namespace Api.FunctionalTests.Common
{
    public sealed class UsersHelper : BaseFunctionalTest
    {
        private static readonly CreateUserRequest _createUserRequest =
            new("", "TestName", "TestLastName", "TestUser", "test@example.com", "Test1234", new DateTime(2000, 10, 10), "+56923147859");

        private static readonly LoginUserRequest _loginUserRequest = new(_createUserRequest.Email, _createUserRequest.Password);

        public UsersHelper(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        public async Task<long> CreateUser()
        {
            HttpResponseMessage createUserResponse = await HttpClient.PostAsJsonAsync(usersBaseUrl, _createUserRequest);

            long userId = await createUserResponse.Content.ReadFromJsonAsync<long>();

            return userId;
        }

        public async Task<LoginResponse?> LoginUser()
        {
            HttpResponseMessage loginUserResponse = await HttpClient.PostAsJsonAsync($"{usersBaseUrl}/login", _loginUserRequest);

            LoginResponse? userResponse = await loginUserResponse.Content.ReadFromJsonAsync<LoginResponse>();

            return userResponse;
        }
    }
}
