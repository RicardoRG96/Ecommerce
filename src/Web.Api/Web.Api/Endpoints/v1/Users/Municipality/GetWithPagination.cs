using Application.Abstractions.Messaging;
using Application.Users.Municipalities.GetWithPagination;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Municipality
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("municipalities", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetMunicipalityWithPaginationQuery, PaginatedList<MunicipalityResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetMunicipalityWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<MunicipalityResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Municipalitites);
        }
    }
}
