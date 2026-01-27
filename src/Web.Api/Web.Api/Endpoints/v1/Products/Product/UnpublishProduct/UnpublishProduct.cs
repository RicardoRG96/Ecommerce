using Application.Abstractions.Messaging;
using Application.Products.Products.UnpublishProduct;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.UnpublishProduct
{
    internal sealed class UnpublishProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("products/{id:long}/unpublish", async (
                long id,
                ICommandHandler<UnpublishProductCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UnpublishProductCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.Unpublish)
            .WithTags(Tags.Products);
        }
    }
}
