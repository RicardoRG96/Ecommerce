using Application.Abstractions.Messaging;
using Application.Products.Attributes.GetById;
using Infrastructure.Access;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.v1.Products.Attribute.Get
{
    internal sealed class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("attributes/{id:long}", async (
                long id,
                IQueryHandler<GetAttributeByIdQuery, AttributeResponse> handler,
                CancellationToken cancellationToken) =>
            {
                GetAttributeByIdQuery query = new(id);

                Result<AttributeResponse> result = await handler.Handle(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .HasPermission(Permissions.Attributes.Read)
            .WithTags(Tags.Attributes);
        }
    }
}
