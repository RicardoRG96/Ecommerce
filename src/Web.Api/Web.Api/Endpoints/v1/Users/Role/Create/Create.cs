using Application.Abstractions.Messaging;
using Application.Users.Roles.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("admin/roles", async (
                CreateRoleRequest request,
                ICommandHandler<CreateRoleCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateRoleCommand command = new(request.RoleName);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.Create)
            .WithTags(Tags.Roles);
        }
    }
}
