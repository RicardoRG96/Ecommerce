using Application.Abstractions.Messaging;
using Application.Users.Addresses.Delete;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address.Delete
{
    internal sealed class Delete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("addresses/{addressId}", async (
                long addressId,
                ICommandHandler<DeleteAddressCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeleteAddressCommand command = new(addressId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.UsersDelete)
            .WithTags(Tags.Addresses);
        }
    }
}
