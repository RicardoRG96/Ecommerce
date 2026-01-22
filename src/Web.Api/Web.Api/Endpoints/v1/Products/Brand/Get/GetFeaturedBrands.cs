using Application.Abstractions.Messaging;
using Application.Products.Brands.GetFeaturedBrands;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Get
{
    internal sealed class GetFeaturedBrands : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("brands/featured", async (
                IQueryHandler<GetFeaturedBrandsQuery, List<BrandResponse>> handler,
                CancellationToken cancellationToken) =>
            {
                GetFeaturedBrandsQuery query = new();

                Result<List<BrandResponse>> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Read)
            .WithTags(Tags.Brands);
        }
    }
}
