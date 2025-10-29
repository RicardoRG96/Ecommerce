using Application.Abstractions.Messaging;
using Application.Users.Regions.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("Regions/{regionId}", async (
                long regionId,
                IQueryHandler<GetRegionByIdQuery, RegionResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetRegionByIdQuery query = new(regionId);

                Result<RegionResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Regions);
        }
    }
}
