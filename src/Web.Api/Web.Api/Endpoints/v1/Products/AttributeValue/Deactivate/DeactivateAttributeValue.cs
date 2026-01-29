using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Deactivate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.AttributeValue.Deactivate
{
    internal sealed class DeactivateAttributeValue : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("attribute-values/{id:long}/deactivate", async (
                long id,
                ICommandHandler<DeactivateAttributeValueCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeactivateAttributeValueCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.AttributeValues.Deactivate)
            .WithTags(Tags.AttributeValues);
        }
    }
}
