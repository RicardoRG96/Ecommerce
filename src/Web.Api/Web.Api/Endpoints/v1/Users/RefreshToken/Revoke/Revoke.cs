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
            app.MapDelete("users/refresh-tokens/{id:long}", async (
                long id,
                ICommandHandler<RevokeRefreshTokenCommand> handler,
                CancellationToken cancellationToken) =>
            {
                RevokeRefreshTokenCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.RefreshTokens);
        }
    }
}
