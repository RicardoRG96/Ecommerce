using Application.Abstractions.Messaging;
using Application.Users.Countries.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country
{
    internal sealed class Create : IEndpoint
    {
        public sealed class Request
        {
            public string Name { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("countries", async (
                Request request,
                ICommandHandler<CreateCountryCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateCountryCommand command = new(request.Name);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Countries);
        }
    }
}
