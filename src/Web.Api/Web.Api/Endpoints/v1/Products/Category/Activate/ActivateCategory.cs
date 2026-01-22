using Application.Abstractions.Messaging;
using Application.Products.Categories.Activate;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Category.Activate
{
    internal sealed class ActivateCategory : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("categories/{id:long}/activate", async (
                long id,
                ICommandHandler<ActivateCategoryCommand> handler,
                CancellationToken cancellationToken) =>
            {
                ActivateCategoryCommand command = new(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Categories.Update)
            .WithTags(Tags.Categories);
        }
    }
}
