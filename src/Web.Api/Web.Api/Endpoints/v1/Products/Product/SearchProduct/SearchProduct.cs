using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Application.Products.Products.SearchProduct;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.SearchProduct
{
    internal sealed class SearchProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/search", async (
                string searchQuery,
                IQueryHandler<SearchProductQuery, IEnumerable<ProductSearchModel>> handler,
                CancellationToken cancellationToken) =>
            {
                SearchProductQuery query = new(searchQuery);

                Result<IEnumerable<ProductSearchModel>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Products);
        }
    }
}
