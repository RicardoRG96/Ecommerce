using Application.Abstractions.Messaging;
using Application.Users.Regions.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("regions", async (
                CreateRegionRequest request,
                ICommandHandler<CreateRegionCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateRegionCommand command = new(request.Name);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Regions);
        }
    }
}
