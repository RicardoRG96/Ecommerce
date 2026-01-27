using Application.Abstractions.Messaging;
using Application.Products.Attributes.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Attribute.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("attributes/{id:long}", async (
                long id,
                UpdateAttributeRequest request,
                ICommandHandler<UpdateAttributeCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateAttributeCommand command = new(
                    id,
                    request.Code,
                    request.Name,
                    request.Description,
                    request.DataType,
                    request.IsVariant,
                    request.IsFilterable,
                    request.IsRequired,
                    request.DisplayOrder
                );

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.Attributes.Update)
            .WithTags(Tags.Attributes);
        }
    }
}
