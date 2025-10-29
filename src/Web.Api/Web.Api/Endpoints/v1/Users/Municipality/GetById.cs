using Application.Abstractions.Messaging;
using Application.Users.Municipalities.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("municipalities/{municipalityId}", async (
                long municipalityId,
                IQueryHandler<GetMunicipalityByIdQuery, MunicipalityResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetMunicipalityByIdQuery query = new(municipalityId);

                Result<MunicipalityResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
