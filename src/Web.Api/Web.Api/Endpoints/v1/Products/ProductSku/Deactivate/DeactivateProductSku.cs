using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Deactivate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductSku.Deactivate
{
    internal sealed class DeactivateProductSku : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("skus/{id:long}/deactivate", async (
                long id,
                ICommandHandler<DeactivateProductSkuCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeactivateProductSkuCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductSkus.Deactivate)
            .WithTags(Tags.ProductSkus);
        }
    }
}
