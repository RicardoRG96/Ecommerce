using Application.Abstractions.Messaging;
using Application.Users.Roles.Delete;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.Delete
{
    internal sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("admin/roles/{roleId:long}", async (
                long roleId,
                ICommandHandler<DeleteRoleCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeleteRoleCommand command = new(roleId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.Delete)
            .WithTags(Tags.Roles);
        }
    }
}
