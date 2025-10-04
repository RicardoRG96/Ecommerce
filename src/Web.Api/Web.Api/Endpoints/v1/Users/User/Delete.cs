
using Application.Abstractions.Messaging;
using Application.Users.Users.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User
{
    internal sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("users/{userId}", async (
                long userId,
                ICommandHandler<DeleteUserCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeleteUserCommand command = new(userId);

                Result result = await handler.Handle(command);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
        }
    }
}
