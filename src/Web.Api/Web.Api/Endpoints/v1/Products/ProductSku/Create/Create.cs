using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductSku.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("skus", async (
                CreateProductSkuRequest request,
                ICommandHandler<CreateProductSkuCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateProductSkuCommand command = new(
                    ProductId: request.ProductId,
                    BarCode: request.BarCode,
                    Price: request.Price,
                    Cost: request.Cost,
                    Weight: request.Weight,
                    Length: request.Length,
                    Width: request.Width,
                    Height: request.Height,
                    IsActive: request.IsActive,
                    DisplayOrder: request.DisplayOrder);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductSkus.Create)
            .WithTags(Tags.ProductSkus);
        }
    }
}
