using Application.Abstractions.Messaging;
using Application.Users.Countries.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country
{
    internal sealed class Update : IEndpoint
    {
        public sealed class Request
        {
            public string Name { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("countries/{countryId}", async (
                long countryId,
                Request request,
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
