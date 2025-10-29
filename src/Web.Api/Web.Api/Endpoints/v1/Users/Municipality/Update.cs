using Application.Abstractions.Messaging;
using Application.Users.Municipalities.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality
{
    internal sealed class Update : IEndpoint
    {
        public sealed class Request
        {
            public long RegionId { get; set; }
            public string Name { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("municipalities/{municipalityId}", async (
                long municipalityId,
                Request request,
                ICommandHandler<UpdateMunicipalityCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateMunicipalityCommand command = new()
                {
                    MunicipalityId = municipalityId,
                    RegionId = request.RegionId,
                    Name = request.Name
                };

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
