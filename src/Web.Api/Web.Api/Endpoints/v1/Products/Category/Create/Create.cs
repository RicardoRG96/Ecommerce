using Application.Abstractions.Messaging;
using Application.Products.Categories.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Category.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("categories", async (
                CreateCategoryRequest request,
                ICommandHandler<CreateCategoryCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateCategoryCommand command = new(
                    request.ParentId,
                    request.Name,
                    request.Description,
                    request.ImageUrl,
                    request.Icon,
                    request.DisplayOrder,
                    request.IsActive,
                    request.IsVisibleInMenu,
                    request.MetaTitle,
                    request.MetaDescription,
                    request.MetaKeywords
                );

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Categories.Create)
            .WithTags(Tags.Categories);
        }
    }
}
