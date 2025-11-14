using Application.Abstractions.Messaging;
using Application.Users.Addresses.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address
{
    internal sealed class Create : IEndpoint
    {
        public sealed class Request
        {
            public long CountryId { get; set; }
            public long MunicipalityId { get; set; }
            public string Title { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string Apartament { get; set; }
            public string Reference { get; set; }
            public string PostalCode { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("addresses", async (
                Request request,
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
