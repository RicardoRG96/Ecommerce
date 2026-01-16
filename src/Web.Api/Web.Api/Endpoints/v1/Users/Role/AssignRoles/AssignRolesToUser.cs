using Application.Abstractions.Messaging;
using Application.Users.Roles.Assign;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.AssignRoles
{
    internal sealed class AssignRolesToUser : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("admin/roles/assign/user/{userId:long}", async (
                long userId,
                AssignRolesToUserRequest request,
                ICommandHandler<AssignRolesToUserCommand> handler,
                CancellationToken cancellationToken) =>
            {
                AssignRolesToUserCommand command = new(request.Roles, userId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Users.AssignRole)
            .WithTags(Tags.Roles);
        }
    }
}
