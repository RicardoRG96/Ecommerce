using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Activate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.AttributeValue.Activate
{
    internal sealed class ActivateAttributeValue : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("attribute-values/{id:long}/activate", async (
                long id,
                ICommandHandler<ActivateAttributeValueCommand> handler,
                CancellationToken cancellationToken) =>
            {
                ActivateAttributeValueCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.AttributeValues.Activate)
            .WithTags(Tags.AttributeValues);
        }
    }
}
