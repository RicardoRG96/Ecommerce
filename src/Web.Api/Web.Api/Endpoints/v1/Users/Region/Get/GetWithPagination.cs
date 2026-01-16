using Application.Abstractions.Messaging;
using Application.Users.Regions.GetWithPagination;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region.Get
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("regions", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetRegionsWithPaginationQuery, PaginatedList<RegionResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetRegionsWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<RegionResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Regions.Read)
            .WithTags(Tags.Regions);
        }
    }
}
