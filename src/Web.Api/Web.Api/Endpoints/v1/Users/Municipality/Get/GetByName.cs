using Application.Abstractions.Messaging;
using Application.Users.Municipalities.GetByName;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality.Get
{
    internal sealed class GetByName : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("municipalities/name/{municipalityName}", async (
                string municipalityName,
                IQueryHandler<GetMunicipalityByNameQuery, MunicipalityResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetMunicipalityByNameQuery query = new(municipalityName);

                Result<MunicipalityResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Municipalities.Read)
            .WithTags(Tags.Municipalitites);
        }
    }
}
