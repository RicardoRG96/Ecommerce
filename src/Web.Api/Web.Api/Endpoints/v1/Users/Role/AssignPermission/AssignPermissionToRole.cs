using Application.Abstractions.Messaging;
using Application.Users.Roles.AssignPermission;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.AssignPermission
{
    internal sealed class AssignPermissionToRole : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("admin/permissions/assign", async (
                AssignPermissionToRoleRequest request,
                ICommandHandler<AssignPermissionToRoleCommand> handler,
                CancellationToken cancellationToken) =>
            {
                AssignPermissionToRoleCommand command = new(request.RoleName, request.PermissionName);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.AssignPermission)
            .WithTags(Tags.Roles);
        }
    }
}
