using Application.Abstractions.Messaging;
using Application.Products.Products.GetAllProductsWithPagination;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.Get
{
    internal sealed class GetAllProductsWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetAllProductsWithPaginationQuery, PaginatedList<ProductResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetAllProductsWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<ProductResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.ReadAllProducts)
            .WithTags(Tags.Products);
        }
    }
}
