using Application.Abstractions.Messaging;
using Application.Users.Addresses.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("addresses", async (
                CreateAddressRequest request,
                ICommandHandler<CreateAddressCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateAddressCommand command = new(
                    request.CountryId,
                    request.MunicipalityId,
                    request.Title,
                    request.City,
                    request.City,
                    request.Number,
                    request.Apartament,
                    request.Reference,
                    request.PostalCode);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Addresses);
        }
    }
}
