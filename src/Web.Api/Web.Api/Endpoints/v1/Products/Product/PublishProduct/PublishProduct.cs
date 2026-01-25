using Application.Abstractions.Messaging;
using Application.Products.Products.PublishProduct;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Product.PublishProduct
{
    internal sealed class PublishProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("products/{id:long}/publish", async (
                long id,
                ICommandHandler<PublishProductCommand> handler,
                CancellationToken cancellationToken) =>
            {
                PublishProductCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.Publish)
            .WithTags(Tags.Products);
        }
    }
}
