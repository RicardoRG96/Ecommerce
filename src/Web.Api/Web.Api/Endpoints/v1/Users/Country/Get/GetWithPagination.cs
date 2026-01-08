using Application.Abstractions.Messaging;
using Application.Users.Countries.GetWithPagination;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country.Get
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("countries", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetCountriesWithPaginationQuery, PaginatedList<CountryResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetCountriesWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<CountryResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Countries.Read)
            .WithTags(Tags.Countries);
        }
    }
}
