using Application.Abstractions.Messaging;
using Application.Users.Countries.Delete;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Country.Delete
{
    internal sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("countries/{countryId}", async (
                long countryId,
                ICommandHandler<DeleteCountryCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeleteCountryCommand command = new(countryId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Countries.Delete)
            .WithTags(Tags.Countries);
        }
    }
}
