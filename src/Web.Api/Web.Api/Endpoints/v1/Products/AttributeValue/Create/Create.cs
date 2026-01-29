using Application.Abstractions.Messaging;
using Application.Products.AttributeValues.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.AttributeValue.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("attribute-values", async (
                CreateAttributeValueRequest request,
                ICommandHandler<CreateAttributeValueCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateAttributeValueCommand command = new(
                    request.AttributeId,
                    request.Value,
                    request.NumericValue,
                    request.BooleanValue,
                    request.DisplayOrder,
                    request.IsActive);

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.AttributeValues.Create)
            .WithTags(Tags.AttributeValues);
        }
    }
}
