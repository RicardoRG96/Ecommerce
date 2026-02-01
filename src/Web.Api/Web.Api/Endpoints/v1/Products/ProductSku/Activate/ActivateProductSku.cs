using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Activate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductSku.Activate
{
    internal sealed class ActivateProductSku : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("skus/{id:long}/activate", async (
                long id,
                ICommandHandler<ActivateProductSkuCommand> handler,
                CancellationToken cancellationToken) =>
            {
                ActivateProductSkuCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductSkus.Activate)
            .WithTags(Tags.ProductSkus);
        }
    }
}
