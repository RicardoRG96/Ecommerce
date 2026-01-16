using Application.Abstractions.Messaging;
using Application.Users.Roles.GetWithPagination;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Role.Get
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("admin/roles", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetRolesWithPaginationQuery, PaginatedList<RoleResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetRolesWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<RoleResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Roles.Read)
            .WithTags(Tags.Roles);
        }
    }
}
