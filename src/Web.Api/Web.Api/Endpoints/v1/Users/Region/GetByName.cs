using Application.Abstractions.Messaging;
using Application.Users.Regions.GetByName;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region
{
    internal sealed class GetByName : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("regions/name/{regionName}", async (
                string regionName,
                IQueryHandler<GetRegionByNameQuery, RegionResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetRegionByNameQuery query = new(regionName);

                Result<RegionResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Regions);
        }
    }
}
