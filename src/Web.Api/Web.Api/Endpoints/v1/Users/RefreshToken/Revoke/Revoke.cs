using Application.Abstractions.Messaging;
using Application.Users.RefreshTokens.Revoke;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.RefreshToken.Revoke
{
    internal sealed class Revoke : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("users/refresh-tokens/{userId:long}", async (
                long userId,
                ICommandHandler<RevokeRefreshTokenCommand> handler,
                CancellationToken cancellationToken) =>
            {
                RevokeRefreshTokenCommand command = new(userId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.RefreshTokens);
        }
    }
}
