using Application.Abstractions.Messaging;
using Application.Users.Regions.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("regions/{regionId}", async (
                long regionId,
                UpdateRegionRequest request,
                ICommandHandler<UpdateRegionCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateRegionCommand command = new(regionId, request.Name);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Regions.Update)
            .WithTags(Tags.Regions);
        }
    }
}
