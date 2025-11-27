using Application.Abstractions.Messaging;
using Application.Users.Countries.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("countries/{countryId}", async (
                long countryId,
                UpdateCountryRequest request,
                ICommandHandler<UpdateCountryCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateCountryCommand command = new(countryId, request.Name);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Countries);
        }
    }
}
