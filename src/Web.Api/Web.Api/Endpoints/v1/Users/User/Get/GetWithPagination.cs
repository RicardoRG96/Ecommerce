using Application.Abstractions.Messaging;
using Application.Users.Users.GetWithPagination;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.User.Get
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("users", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetUsersWithPaginationQuery, PaginatedList<UserResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetUsersWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<UserResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Users.Read)
            .WithTags(Tags.Users);
        }
    }
}
