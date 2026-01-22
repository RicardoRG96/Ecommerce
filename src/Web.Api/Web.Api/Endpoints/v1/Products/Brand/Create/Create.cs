using Application.Abstractions.Messaging;
using Application.Products.Brands.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Brand.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("brands", async (
                CreateBrandRequest request,
                ICommandHandler<CreateBrandCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateBrandCommand command = new(
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

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Brands.Create)
            .WithTags(Tags.Brands);
        }
    }
}
