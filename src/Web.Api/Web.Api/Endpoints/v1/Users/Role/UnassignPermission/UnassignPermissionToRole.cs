using Application.Abstractions.Messaging;
using Application.Users.Roles.UnassignPermission;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.UnassignPermission
{
    internal sealed class UnassignPermissionToRole : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("admin/roles/permissions/unassign", async (
                UnassignPermissionToRoleRequest request,
                ICommandHandler<UnassignPermissionToRoleCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UnassignPermissionToRoleCommand command = new(request.RoleName, request.PermissionName);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.UnassignPermission)
            .WithTags(Tags.Roles);
        }
    }
}
