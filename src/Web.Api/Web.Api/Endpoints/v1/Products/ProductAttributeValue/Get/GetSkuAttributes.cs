using Application.Abstractions.Messaging;
using Application.Products.ProductAttributeValues.Common.Mappers;
using Application.Products.ProductAttributeValues.GetSkuAttributes;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductAttributeValue.Get
{
    internal sealed class GetSkuAttributes : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("skus/{skuId:long}/attributes", async (
                long skuId,
                IQueryHandler<GetSkuAttributesQuery, List<ProductAttributeValueResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetSkuAttributesQuery query = new(skuId);

                Result<List<ProductAttributeValueResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductAttributeValues.Read)
            .WithTags(Tags.ProductAttributeValues);
        }
    }
}
