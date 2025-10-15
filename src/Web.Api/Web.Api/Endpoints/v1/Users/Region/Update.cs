using Application.Abstractions.Messaging;
using Application.Users.Regions.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region
{
    public class Update : IEndpoint
    {
        public sealed class Request
        {
            public string Name { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("regions/{regionId}", async (
                long regionId,
                Request request,
                ICommandHandler<UpdateRegionCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateRegionCommand command = new(regionId, request.Name);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Regions);
        }
    }
}
