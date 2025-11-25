using Application.Abstractions.Messaging;
using Application.Users.Municipalities.GetWithPagination;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality.Get
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("municipalities", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetMunicipalitiesWithPaginationQuery, PaginatedList<MunicipalityResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetMunicipalitiesWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<MunicipalityResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
