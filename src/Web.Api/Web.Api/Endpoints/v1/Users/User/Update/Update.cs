using Application.Abstractions.Messaging;
using Application.Users.Users.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("users/{userId}", async (
                long userId,
                UpdateUserRequest request,
                ICommandHandler<UpdateUserCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateUserCommand command = new(
                    userId,
                    request.Avatar,
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Users.Update)
            .WithTags(Tags.Users);
        }
    }
}
