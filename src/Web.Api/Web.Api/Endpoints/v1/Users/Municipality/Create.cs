using Application.Abstractions.Messaging;
using Application.Users.Municipalities.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality
{
    internal sealed class Create : IEndpoint
    {
        public sealed class Request
        {
            public long RegionId { get; set; }
            public string Name { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("municipalities", async (
                Request request,
                ICommandHandler<CreateMunicipalityCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateMunicipalityCommand command = new()
                {
                    RegionId = request.RegionId,
                    Name = request.Name,
                };

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
