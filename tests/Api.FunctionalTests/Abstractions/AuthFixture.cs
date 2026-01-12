using Application.Users.Users.Login;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http.Json;
using Web.Api.Endpoints.v1.Users.User.Create;
using Web.Api.Endpoints.v1.Users.User.Login;

namespace Api.FunctionalTests.Abstractions
{
    public sealed class AuthFixture
    {
        public class AdminUser
        {
            private static readonly CreateUserRequest _createUserRequest =
            new("", "Admin", "User", "AdminUser", "testAdmin@example.com", "Admin1234", new DateTime(1990, 11, 12), "+56912345678");

            private static readonly LoginUserRequest _loginUserRequest = new(_createUserRequest.Email, _createUserRequest.Password);

            private readonly UserManager<ApplicationUser> _userManager;
            private const string _roleName = "Admin";

            public long UserId { get; private set; } = default!;
            public string AccessToken { get; private set; } = default!;
            public string RefreshToken { get; private set; } = default!;

            public AdminUser(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task InitializeAsync(HttpClient client)
            {
                await EnsureAdminUserExistsAsync(client);

                LoginResponse tokens = await LoginAsync(client);

                AccessToken = tokens.AccessToken;
                RefreshToken = tokens.RefreshToken;
            }

            private async Task EnsureAdminUserExistsAsync(HttpClient client)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/users", _createUserRequest);

                if (response.StatusCode is HttpStatusCode.Conflict)
                {
                    return;
                }

                response.EnsureSuccessStatusCode();

                UserId = await response.Content.ReadFromJsonAsync<long>();

                ApplicationUser? user = await _userManager.FindByIdAsync(UserId.ToString());

                await _userManager.AddToRoleAsync(user!, _roleName);
            }

            private async Task<LoginResponse> LoginAsync(HttpClient client)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/users/login", _loginUserRequest);

                response.EnsureSuccessStatusCode();

                LoginResponse? result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result;
            }
        }

        public class CustomerUser
        {
            private static readonly CreateUserRequest _createUserRequest =
            new("", "TestName", "TestLastName", "TestUser", "test@example.com", "Test1234", new DateTime(2000, 10, 10), "+56923147859");

            private static readonly LoginUserRequest _loginUserRequest = new(_createUserRequest.Email, _createUserRequest.Password);

            public long UserId { get; private set; } = default!;
            public string AccessToken { get; private set; } = default!;
            public string RefreshToken { get; private set; } = default!;

            public async Task InitializeAsync(HttpClient client)
            {
                await EnsureUserExistsAsync(client);

                LoginResponse tokens = await LoginAsync(client);

                AccessToken = tokens.AccessToken;
                RefreshToken = tokens.RefreshToken;
            }

            private async Task EnsureUserExistsAsync(HttpClient client)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/users", _createUserRequest);

                if (response.StatusCode is HttpStatusCode.Conflict)
                {
                    return;
                }

                response.EnsureSuccessStatusCode();

                UserId = await response.Content.ReadFromJsonAsync<long>();
            }

            private async Task<LoginResponse> LoginAsync(HttpClient client)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/users/login", _loginUserRequest);

                response.EnsureSuccessStatusCode();

                LoginResponse? result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result;
            }
        }
    }
}
