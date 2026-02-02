using Application.Abstractions.Messaging;
using Application.Products.ProductAttributeValues.Remove;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductAttributeValue.Remove
{
    internal sealed class Remove : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("skus/{skuId:long}/attributes/{attributeValueId:long}", async (
                long skuId,
                long attributeValueId,
                ICommandHandler<RemoveAttributeValueFromSkuCommand> handler,
                CancellationToken cancellationToken) =>
            {
                RemoveAttributeValueFromSkuCommand command = new(
                    SkuId: skuId,
                    AttributeValueId: attributeValueId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductAttributeValues.UnassignFromSku)
            .WithTags(Tags.ProductAttributeValues);
        }
    }
}
