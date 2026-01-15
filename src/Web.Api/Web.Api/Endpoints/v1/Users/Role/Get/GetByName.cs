using Application.Abstractions.Messaging;
using Application.Users.Roles.GetByName;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.Get
{
    internal sealed class GetByName : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("admin/roles/name/{rolename}", async (
                string roleName,
                IQueryHandler<GetRoleByNameQuery, RoleResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetRoleByNameQuery query = new(roleName);

                Result<RoleResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.Read)
            .WithTags(Tags.Roles);
        }
    }
}
