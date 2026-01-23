using Application.Abstractions.Messaging;
using Application.Products.Categories.Deactivate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Category.Deactivate
{
    internal sealed class DeactivateCategory : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("categories/{id:long}/deactivate", async (
                long id,
                ICommandHandler<DeactivateCategoryCommand> handler,
                CancellationToken cancellationToken) =>
            {
                DeactivateCategoryCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Categories.Update)
            .WithTags(Tags.Categories);
        }
    }
}
