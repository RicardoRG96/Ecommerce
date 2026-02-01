using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Common.Mappers;
using Application.Products.ProductSkus.GetById;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductSku.Get
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("skus/{id:long}", async (
                long id,
                IQueryHandler<GetProductSkuByIdQuery, ProductSkuResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetProductSkuByIdQuery query = new(id);

                Result<ProductSkuResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductSkus.Read)
            .WithTags(Tags.ProductSkus);
        }
    }
}
