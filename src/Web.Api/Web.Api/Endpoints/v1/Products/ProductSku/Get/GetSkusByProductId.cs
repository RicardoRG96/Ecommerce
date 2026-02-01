using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Mappers;
using Application.Products.ProductSkus.GetSkusByProduct;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductSku.Get
{
    internal sealed class GetSkusByProductId : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/{productId:long}/skus", async (
                long productId,
                IQueryHandler<GetSkusByProductQuery, List<ProductSkuResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetSkusByProductQuery query = new(productId);

                Result<List<ProductSkuResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductSkus.Read)
            .WithTags(Tags.ProductSkus);
        }
    }
}
