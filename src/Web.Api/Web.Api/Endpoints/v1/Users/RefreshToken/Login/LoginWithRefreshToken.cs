using Application.Abstractions.Messaging;
using Application.Users.RefreshTokens.Login;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.RefreshToken.Login
{
    internal sealed class LoginWithRefreshToken : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/refresh-tokens", async (
                LoginWithRefreshTokenRequest request,
                ICommandHandler<LoginWithRefreshTokenCommand, RefreshTokenResponse> handler,
                CancellationToken cancellationToken) =>
            {
                LoginWithRefreshTokenCommand command = new(request.RefreshToken, request.UserId);

                Result<RefreshTokenResponse> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.RefreshTokens);
        }
    }
}
