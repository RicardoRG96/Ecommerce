using Application.Abstractions.Messaging;
using Application.Products.Categories.ChangeCategoryParent;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Category.ChangeParent
{
    internal sealed class ChangeParent : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("categories/{id:long}/parent", async (
                long id,
                ChangeParentRequest request,
                ICommandHandler<ChangeCategoryParentCommand> handler,
                CancellationToken cancellationToken) =>
            {
                ChangeCategoryParentCommand command = new(
                    id,
                    request.NewParentId
                );

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Categories.Update)
            .WithTags(Tags.Categories);
        }
    }
}
