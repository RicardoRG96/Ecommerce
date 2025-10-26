using Application.Abstractions.Messaging;
using Application.Users.Municipalities.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality
{
    public class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("Municipalities/{municipalityId}", async (
                long municipalityId,
                ICommandHandler<DeleteMunicipalityCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeleteMunicipalityCommand command = new(municipalityId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
