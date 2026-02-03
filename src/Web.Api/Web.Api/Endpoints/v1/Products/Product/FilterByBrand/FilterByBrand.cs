using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Application.Products.Products.FilterByBrand;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.FilterByBrand
{
    internal sealed class FilterByBrand : IEndpoint
    {
        public record BrandQueriesRequest(
            [FromQuery] List<string> Brands);

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/filter-by-brand", async (
                string[] brands,
                IQueryHandler<FilterByBrandQuery, IEnumerable<ProductSearchModel>> handler,
                CancellationToken cancellationToken) =>
            {
                List<string> formatedQuery = [.. brands];

                FilterByBrandQuery query = new(formatedQuery);

                Result<IEnumerable<ProductSearchModel>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Products);
        }
    }
}
