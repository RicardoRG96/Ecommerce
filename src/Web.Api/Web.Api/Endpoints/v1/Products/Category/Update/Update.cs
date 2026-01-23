using Application.Abstractions.Messaging;
using Application.Products.Categories.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Category.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("categories/{id:long}", async (
                long id,
                UpdateCategoryRequest request,
                ICommandHandler<UpdateCategoryCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateCategoryCommand command = new(
                    id,
                    request.Name,
                    request.Description,
                    request.ImageUrl,
                    request.Icon,
                    request.DisplayOrder,
                    request.IsVisibleInMenu,
                    request.MetaTitle,
                    request.MetaDescription,
                    request.MetaKeywords
                );

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Categories.Update)
            .WithTags(Tags.Categories);
        }
    }
}
