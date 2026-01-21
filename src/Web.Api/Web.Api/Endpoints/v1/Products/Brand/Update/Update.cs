using Application.Abstractions.Messaging;
using Application.Products.Brands.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("products/brands/{brandId:logn}", async (
                long brandId,
                UpdateBrandRequest request,
                ICommandHandler<UpdateBrandCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateBrandCommand command = new(
                    brandId,
                    request.Name,
                    request.Description,
                    request.LogoUrl,
                    request.BannerUrl,
                    request.WebsiteUrl,
                    request.IsActive,
                    request.IsFeatured,
                    request.DisplayOrder,
                    request.MetaTitle,
                    request.MetaDescription,
                    request.MetaKeywords);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Update)
            .WithTags(Tags.Brands);
        }
    }
}
