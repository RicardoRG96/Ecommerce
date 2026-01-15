using Application.Abstractions.Messaging;
using Application.Users.Roles.GetById;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.Get
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("admin/roles/{roleId:long}", async (
                long roleId,
                IQueryHandler<GetRoleByIdQuery, RoleResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetRoleByIdQuery query = new(roleId);

                Result<RoleResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.Read)
            .WithTags(Tags.Roles);
        }
    }
}
