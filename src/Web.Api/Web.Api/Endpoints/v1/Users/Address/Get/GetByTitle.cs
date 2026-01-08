using Application.Abstractions.Messaging;
using Application.Users.Addresses;
using Application.Users.Addresses.GetByTitle;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address.Get
{
    internal sealed class GetByTitle : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("addresses/title/{title}", async (
                string title,
                IQueryHandler<GetAddressByTitleQuery, AddressResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetAddressByTitleQuery query = new(title);

                Result<AddressResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Addresses.Read)
            .WithTags(Tags.Addresses);
        }
    }
}
