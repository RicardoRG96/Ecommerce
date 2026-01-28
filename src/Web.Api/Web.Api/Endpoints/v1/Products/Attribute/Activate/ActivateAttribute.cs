using Application.Abstractions.Messaging;
using Application.Products.Attributes.Activate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Attribute.Activate
{
    internal sealed class ActivateAttribute : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("attributes/{id:long}/activate", async (
                long id,
                ICommandHandler<ActivateAttributeCommand> handler,
                CancellationToken cancellationToken) =>
            {
                ActivateAttributeCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Attributes.Activate)
            .WithTags(Tags.Attributes);
        }
    }
}
