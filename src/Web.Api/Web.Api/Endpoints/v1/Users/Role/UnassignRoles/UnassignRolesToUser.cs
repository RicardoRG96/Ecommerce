using Application.Abstractions.Messaging;
using Application.Users.Roles.Unassign;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.UnassignRoles
{
    internal sealed class UnassignRolesToUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("admin/roles/unassign/user/{userId:long}", async (
                long userId,
                UnassignRolesToUserRequest request,
                ICommandHandler<UnassignRolesToUserCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UnassignRolesToUserCommand command = new(request.Roles, userId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Users.UnassignRole)
            .WithTags(Tags.Roles);
        }
    }
}
