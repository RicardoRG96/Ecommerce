using Application.Abstractions.Messaging;
using Application.Users.Addresses;
using Application.Users.Addresses.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address.Get
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("addresses/{addressId}", async (
                long addressId,
                IQueryHandler<GetAddressByIdQuery, AddressResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetAddressByIdQuery query = new(addressId);

                Result<AddressResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Addresses);
        }
    }
}
