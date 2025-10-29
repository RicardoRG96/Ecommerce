using Application.Abstractions.Messaging;
using Application.Users.Addresses;
using Application.Users.Addresses.GetWithPagination;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("addresses", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetAddressesWithPaginationQuery, PaginatedList<AddressResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetAddressesWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<AddressResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Addresses);
        }
    }
}
