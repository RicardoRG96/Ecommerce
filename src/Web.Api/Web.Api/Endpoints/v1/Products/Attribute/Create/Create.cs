using Application.Abstractions.Messaging;
using Application.Products.Attributes.Create;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Attribute.Create
{
    internal sealed class Create : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("attributes", async (
                CreateAttributeRequest request,
                ICommandHandler<CreateAttributeCommand, long> handler,
                CancellationToken cancellationToken) =>
            {
                CreateAttributeCommand command = new(
                    request.Code,
                    request.Name,
                    request.Description,
                    request.DataType,
                    request.IsVariant,
                    request.IsFilterable,
                    request.IsRequired,
                    request.DisplayOrder,
                    request.IsActive
                );

                Result<long> result = await handler.Handle(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Attributes.Create)
            .WithTags(Tags.Attributes);
        }
    }
}
