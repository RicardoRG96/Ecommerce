using Application.Abstractions.Messaging;
using Application.Products.Brands.GetWithPagination;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Get
{
    internal sealed class GetWithPagination : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/brands", async (
                int pageNumber,
                int pageSize,
                IQueryHandler<GetBrandsWithPaginationQuery, PaginatedList<BrandResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetBrandsWithPaginationQuery query = new(pageNumber, pageSize);

                Result<PaginatedList<BrandResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Read)
            .WithTags(Tags.Brands);
        }
    }
}
