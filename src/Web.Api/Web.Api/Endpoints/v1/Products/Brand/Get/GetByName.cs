using Application.Abstractions.Messaging;
using Application.Products.Brands.GetByName;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Get
{
    internal sealed class GetByName : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/brands/name/{brandName}", async (
                string brandName,
                IQueryHandler<GetBrandByNameQuery, BrandResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetBrandByNameQuery query = new(brandName);

                Result<BrandResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Read)
            .WithTags(Tags.Brands);
        }
    }
}
