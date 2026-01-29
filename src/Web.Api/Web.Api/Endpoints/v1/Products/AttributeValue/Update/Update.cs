using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Update;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.AttributeValue.Update
{
    internal sealed class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("attribute-values/{id:long}", async (
                long id,
                UpdateAttributeValueRequest request,
                ICommandHandler<UpdateAttributeValueCommand> handler,
                CancellationToken cancellationToken) =>
            {
                UpdateAttributeValueCommand command = new(
                    id,
                    request.AttributeId,
                    request.Value,
                    request.NumericValue,
                    request.BooleanValue,
                    request.DisplayOrder);

                Result result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .HasPermission(Permissions.AttributeValues.Update)
            .WithTags(Tags.AttributeValues);
        }
    }
}
