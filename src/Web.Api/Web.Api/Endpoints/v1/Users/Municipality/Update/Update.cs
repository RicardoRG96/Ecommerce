using Application.Abstractions.Messaging;
using Application.Users.Municipalities.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("municipalities/{municipalityId}", async (
                long municipalityId,
                UpdateMunicipalityRequest request,
                ICommandHandler<UpdateMunicipalityCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateMunicipalityCommand command = new(
                    municipalityId,
                    request.RegionId,
                    request.Name);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
