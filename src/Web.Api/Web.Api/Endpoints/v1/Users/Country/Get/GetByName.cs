using Application.Abstractions.Messaging;
using Application.Users.Countries.GetByName;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country.Get
{
    internal sealed class GetByName : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("countries/name/{name}", async (
                string name,
                IQueryHandler<GetCountryByNameQuery, CountryResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetCountryByNameQuery query = new(name);

                Result<CountryResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Countries);
        }
    }
}
