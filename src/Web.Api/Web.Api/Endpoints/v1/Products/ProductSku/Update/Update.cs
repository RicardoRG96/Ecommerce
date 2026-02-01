using Application.Abstractions.Messaging;
using Application.Products.ProductSkus.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.ProductSku.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("skus/{id:long}", async (
                long id,
                UpdateProductSkuRequest request,
                ICommandHandler<UpdateProductSkuCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateProductSkuCommand command = new(
                    Id: id,
                    ProductId: request.ProductId,
                    BarCode: request.BarCode,
                    Price: request.Price,
                    Cost: request.Cost,
                    Weight: request.Weight,
                    Length: request.Length,
                    Width: request.Width,
                    Height: request.Height,
                    DisplayOrder: request.DisplayOrder
                );

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.ProductSkus.Update)
            .WithTags(Tags.ProductSkus);
        }
    }
}
