using Application.Abstractions.Messaging;
using Application.Users.Regions.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Region
{
    internal sealed class Create : IEndpoint
    {
        public sealed class Request
        {
            public string Name { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("regions", async (
                Request request,
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
