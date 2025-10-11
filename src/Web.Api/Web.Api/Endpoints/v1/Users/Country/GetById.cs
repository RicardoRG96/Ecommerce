
using Application.Abstractions.Messaging;
using Application.Users.Countries.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("countries/{countryId}", async (
                long countryId,
                IQueryHandler<GetCountryByIdQuery, CountryResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetCountryByIdQuery query = new(countryId);

                Result<CountryResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Countries);
        }
    }
}
