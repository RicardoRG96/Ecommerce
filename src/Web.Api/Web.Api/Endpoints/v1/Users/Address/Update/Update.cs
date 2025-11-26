using Application.Abstractions.Messaging;
using Application.Users.Addresses.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("addresses/{addressId}", async (
                long addressId,
                UpdateAddressRequest request,
                ICommandHandler<UpdateAddressCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateAddressCommand command = new(
                    addressId,
                    request.MunicipalityId,
                    request.Title,
                    request.City,
                    request.City,
                    request.Number,
                    request.Apartament,
                    request.Reference,
                    request.PostalCode);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Addresses);
        }
    }
}
