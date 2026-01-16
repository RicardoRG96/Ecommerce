using Application.Abstractions.Messaging;
using Application.Users.Roles.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("admin/roles/{roleId:long}", async (
                long roleId,
                UpdateRoleRequest request,
                ICommandHandler<UpdateRoleCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateRoleCommand command = new(roleId, request.Name);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.Update)
            .WithTags(Tags.Roles);
        }
    }
}
