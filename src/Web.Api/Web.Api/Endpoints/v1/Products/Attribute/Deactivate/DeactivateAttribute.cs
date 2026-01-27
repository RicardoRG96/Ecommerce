using Application.Abstractions.Messaging;
using Application.Products.Attributes.Deactivate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Attribute.Deactivate
{
    internal sealed class DeactivateAttribute : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("attributes/{id:long}/deactivate", async (
                long id,
                ICommandHandler<DeactivateAttributeCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeactivateAttributeCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Attributes.Deactivate)
            .WithTags(Tags.Attributes);
        }
    }
}
