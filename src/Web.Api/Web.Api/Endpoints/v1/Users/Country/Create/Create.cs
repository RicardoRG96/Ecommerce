using Application.Abstractions.Messaging;
using Application.Users.Countries.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("countries", async (
                CreateCountryRequest request,
                ICommandHandler<CreateCountryCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateCountryCommand command = new(request.Name);
                
                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Countries.Create)
            .WithTags(Tags.Countries);
        }
    }
}
