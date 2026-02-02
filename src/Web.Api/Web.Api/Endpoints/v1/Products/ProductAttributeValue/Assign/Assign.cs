using Application.Abstractions.Messaging;
using Application.Products.ProductAttributeValues.Assign;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductAttributeValue.Assign
{
    internal sealed class Assign : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/skus/{skuId:long}/attributes", async (
                long skuId,
                AssignRequest request,
                ICommandHandler<AssignAttributeValueToSkuCommand> handler,
                CancellationToken cancellationToken) =>
            {
                AssignAttributeValueToSkuCommand command = new(
                    SkuId: skuId,
                    AttributeValueId: request.AttributeValueId);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductAttributeValues.AssignToSku)
            .WithTags(Tags.ProductAttributeValues);
        }
    }
}
