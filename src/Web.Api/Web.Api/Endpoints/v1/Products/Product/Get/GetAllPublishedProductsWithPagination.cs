using Application.Abstractions.Messaging;
using Application.Products.Products.GetAllPublishedProductsWithPagination;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.Get
{
    internal sealed class GetAllPublishedProductsWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/published", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetAllPublishedProductsWithPaginationQuery, PaginatedList<PublishedProductResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetAllPublishedProductsWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<PublishedProductResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.Read)
            .WithTags(Tags.Products);
        }
    }
}
