using Application.Abstractions.Messaging;
using Application.Products.Products.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("products/{id:long}", async (
                long id,
                UpdateProductRequest request,
                ICommandHandler<UpdateProductCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateProductCommand command = new(
                    id,
                    request.Name,
                    request.Description,
                    request.ShortDescription,
                    request.BrandId,
                    request.CategoryId,
                    request.ProductTaxCategoryId,
                    request.IsFeatured,
                    request.IsDigital,
                    request.MetaTitle,
                    request.MetaDescription,
                    request.MetaKeywords);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.Update)
            .WithTags(Tags.Products);
        }
    }
}
