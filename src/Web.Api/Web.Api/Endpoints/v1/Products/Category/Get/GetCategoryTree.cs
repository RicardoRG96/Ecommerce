using Application.Abstractions.Messaging;
using Application.Products.Categories.GetCategoryTree;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Category.Get
{
    internal sealed class GetCategoryTree : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("categories/tree", async (
                IQueryHandler<GetCategoryTreeQuery, List<CategoryTreeResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetCategoryTreeQuery query = new();

                Result<List<CategoryTreeResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Categories.Read)
            .WithTags(Tags.Categories);
        }
    }
}
