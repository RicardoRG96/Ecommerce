using Application.Abstractions.Messaging;
using Application.Products.IndexProducts;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.IndexProducts
{
    internal sealed class IndexProducts : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("products/index-search", async (
                ICommandHandler<IndexProductsCommand> handler,
                CancellationToken cancellationToken) =>
            {
                IndexProductsCommand command = new();

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Products.IndexProductsToSearch)
            .WithTags(Tags.Products);
        }
    }
}
