using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Application.Products.Products.FilterByPriceRange;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.FilterByPriceRange
{
    internal sealed class FilterByPriceRange : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/filter-by-price-range", async (
                decimal minPrice,
                decimal maxPrice,
                IQueryHandler<FilterByPriceRangeQuery, IEnumerable<ProductSearchModel>> handler,
                CancellationToken cancellationToken) =>
            {
                FilterByPriceRangeQuery query = new(minPrice, maxPrice);

                Result<IEnumerable<ProductSearchModel>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Products);
        }
    }
}
