using Application.Abstractions.Messaging;
using Application.Products.Brands.GetById;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Get
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("brands/{brandId:long}", async (
                long brandId,
                IQueryHandler<GetBrandByIdQuery, BrandResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetBrandByIdQuery query = new(brandId);

                Result<BrandResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Read)
            .WithTags(Tags.Brands);
        }
    }
}
