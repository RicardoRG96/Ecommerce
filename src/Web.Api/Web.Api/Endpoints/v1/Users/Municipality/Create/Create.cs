using Application.Abstractions.Messaging;
using Application.Users.Municipalities.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("municipalities", async (
                CreateMunicipalityRequest request,
                ICommandHandler<CreateMunicipalityCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateMunicipalityCommand command = new(
                    request.RegionId,
                    request.Name);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
