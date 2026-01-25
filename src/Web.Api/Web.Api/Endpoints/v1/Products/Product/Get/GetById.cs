using Application.Abstractions.Messaging;
using Application.Products.Products.GetProductDetailById;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.Get
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/{id:long}", async (
                long id,
                IQueryHandler<GetProductDetailByIdQuery, ProductDetailResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetProductDetailByIdQuery query = new(id);

                Result<ProductDetailResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.Read)
            .WithTags(Tags.Products);
        }
    }
}
