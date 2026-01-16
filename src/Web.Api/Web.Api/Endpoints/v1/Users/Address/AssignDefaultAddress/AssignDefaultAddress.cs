using Application.Abstractions.Messaging;
using Application.Users.Addresses.AssignDefaultAddress;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Users.Address.AssignDefaultAddress
{
    internal sealed class AssignDefaultAddress : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("addresses/me/default-address/{userId:long}", async (
                long userId,
                AssignDefaultAddressRequest request,
                ICommandHandler<AssignDefaultAddressCommand> handler,
                CancellationToken cancellationtoken) =>
            {
                AssignDefaultAddressCommand command = new(userId, request.AddressId);

                Result result = await handler.Handle(command, cancellationtoken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Addresses.AssignDefaultAddress)
            .WithTags(Tags.Addresses);
        }
    }
}
