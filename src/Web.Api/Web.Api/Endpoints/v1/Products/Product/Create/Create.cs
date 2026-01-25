using Application.Abstractions.Messaging;
using Application.Products.Products.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("products", async (
                CreateProductRequest request,
                ICommandHandler<CreateProductCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateProductCommand command = new(
                    request.Name,
                    request.Description,
                    request.ShortDescription,
                    request.BrandId,
                    request.CategoryId,
                    request.ProductTaxCategoryId,
                    request.IsActive,
                    request.IsFeatured,
                    request.IsDigital,
                    request.MetaTitle,
                    request.MetaDescription,
                    request.MetaKeywords);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.Create)
            .WithTags(Tags.Products);
        }
    }
}
