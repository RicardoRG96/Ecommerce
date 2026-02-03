using Application.Abstractions.Messaging;
using Application.Abstractions.Search;
using Application.Products.Products.FilterByCategory;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.FilterByCategory
{
    internal sealed class FilterByCategory : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/filter-by-categories", async (
                string[] categories,
                IQueryHandler<FilterByCategoryQuery, IEnumerable<ProductSearchModel>> handler,
                CancellationToken cancellationToken) =>
            {
                List<string> formatedQuery = [.. categories];

                FilterByCategoryQuery query = new(formatedQuery);

                Result<IEnumerable<ProductSearchModel>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Products);
        }
    }
}
