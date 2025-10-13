using Application.Abstractions.Messaging;
using Application.Users.Regions.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region
{
    public class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("regions/{regionId}", async (
                long regionId,
                ICommandHandler<DeleteRegionCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeleteRegionCommand command = new(regionId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Regions);
        }
    }
}
